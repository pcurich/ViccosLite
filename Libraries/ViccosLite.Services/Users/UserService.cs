using System;
using System.Collections.Generic;
using System.Linq;
using ViccosLite.Core;
using ViccosLite.Core.Caching;
using ViccosLite.Core.Data;
using ViccosLite.Core.Domain.Users;
using ViccosLite.Data.Entities;

namespace ViccosLite.Services.Users
{
    public class UserService : IUserService
    {
        #region Ctr

        public UserService(IRepository<User> userRepository, IRepository<UserRole> userRoleRepository,
            IDataProvider dataProvider, ISoftContext softContext, ICacheManager cacheManager)
        {
            _userRepository = userRepository;
            _userRoleRepository = userRoleRepository;
            _dataProvider = dataProvider;
            _softContext = softContext;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Constantes

        /// <summary>
        ///     Key for caching
        /// </summary>
        /// <remarks>
        ///     {0} : show hidden records?
        /// </remarks>
        private const string USERROLES_ALL_KEY = "Soft.userrole.all-{0}";

        /// <summary>
        ///     Key for caching
        /// </summary>
        /// <remarks>
        ///     {0} : system name
        /// </remarks>
        private const string USERROLES_BY_SYSTEMNAME_KEY = "Soft.userrole.systemname-{0}";

        /// <summary>
        ///     Key pattern to clear cache
        /// </summary>
        private const string USERROLES_PATTERN_KEY = "Soft.userrole.";

        #endregion

        #region Campos

        private readonly IRepository<User> _userRepository;
        private readonly IRepository<UserRole> _userRoleRepository;

        private readonly IDataProvider _dataProvider;
        private readonly ISoftContext _softContext;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Metodos

        #region Usuarios

        public virtual IPagedList<User> GetAllUsers(int pageIndex, int pageSize, int[] userRoleIds = null,
            string email = null, string username = null,
            string firstName = null, string lastName = null)
        {
            var query = _userRepository.Table
                .Where(c => !c.Deleted);
            if (userRoleIds != null && userRoleIds.Length > 0)
                query = query.Where(c => c.UserRoles.Select(cr => cr.Id).Intersect(userRoleIds).Any());
            if (!String.IsNullOrWhiteSpace(email))
                query = query.Where(c => c.Email.Contains(email));
            if (!String.IsNullOrWhiteSpace(username))
                query = query.Where(c => c.UserName.Contains(username));
            if (!String.IsNullOrWhiteSpace(firstName))
                query = query.Where(c => c.FirstName.Contains(firstName));
            if (!String.IsNullOrWhiteSpace(lastName))
                query = query.Where(c => c.LastName.Contains(lastName));

            query = query.OrderByDescending(c => c.UserName);

            var users = new PagedList<User>(query, pageIndex, pageSize);
            return users;
        }

        public virtual void DeleteUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            if (user.IsSystemAccount)
                throw new KsException(string.Format("System user account ({0}) could not be deleted", user.SystemName));

            user.Deleted = true;
            user.Active = false;

            if (!String.IsNullOrEmpty(user.Email))
                user.Email += "-DELETED";
            if (!String.IsNullOrEmpty(user.UserName))
                user.UserName += "-DELETED";
            UpdateUser(user);
        }

        public virtual User GetUserById(int userId)
        {
            return userId == 0 ? null : _userRepository.GetById(userId);
        }

        public virtual IList<User> GetUserByIds(int[] userId)
        {
            if (userId == null || userId.Length == 0)
                return new List<User>();

            var query = from c in _userRepository.Table
                where userId.Contains(c.Id)
                select c;
            var users = query.ToList();
            //sort by passed identifiers
            var sortedUsers = new List<User>();
            foreach (var id in userId)
            {
                var user = users.Find(x => x.Id == id);
                if (user != null)
                    sortedUsers.Add(user);
            }
            return sortedUsers;
        }

        public virtual User GetUserByUserName(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                return null;

            var query = from c in _userRepository.Table
                where c.UserName == username && (c.Active || !c.Deleted)
                //para que al ser eliminados puedan volverse a registrar
                orderby c.Id
                select c;
            var user = query.FirstOrDefault();
            return user;
        }

        public virtual User GetUserByEmail(string email)
        {
            if (string.IsNullOrWhiteSpace(email))
                return null;

            var query = from c in _userRepository.Table
                        where c.Email == email && (c.Active || !c.Deleted)
                        //para que al ser eliminados puedan volverse a registrar
                        orderby c.Id
                        select c;
            var user = query.FirstOrDefault();
            return user;
        }

        public virtual User GetUserBySystemName(string systemName)
        {
            if (string.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from c in _userRepository.Table
                where c.SystemName == systemName
                orderby c.Id
                select c;
            var user = query.FirstOrDefault();
            return user;
        }

        public virtual void InsertUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Insert(user);
        }

        public virtual void UpdateUser(User user)
        {
            if (user == null)
                throw new ArgumentNullException("user");

            _userRepository.Update(user);
        }

        #endregion

        #region Roles del usuario

        public virtual void DeleteUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException("userRole");

            if (userRole.IsSystemRole)
                throw new KsException("System role could not be deleted");

            _userRoleRepository.Delete(userRole);

            _cacheManager.RemoveByPattern(USERROLES_PATTERN_KEY);
        }

        public virtual UserRole GetUserRoleById(int userRoleId)
        {
            if (userRoleId == 0)
                return null;

            return _userRoleRepository.GetById(userRoleId);
        }

        public virtual UserRole GetUserRoleBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            var key = string.Format(USERROLES_BY_SYSTEMNAME_KEY, systemName);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _userRoleRepository.Table
                    orderby cr.Id
                    where cr.SystemName == systemName
                    select cr;
                var userRole = query.FirstOrDefault();
                return userRole;
            });
        }

        public virtual IList<UserRole> GetAllUserRoles(bool showHidden = false)
        {
            var key = string.Format(USERROLES_ALL_KEY, showHidden);
            return _cacheManager.Get(key, () =>
            {
                var query = from cr in _userRoleRepository.Table
                    orderby cr.UserRoleName
                    where (showHidden || cr.Active)
                    select cr;
                var customerRoles = query.ToList();
                return customerRoles;
            });
        }

        public virtual void InsertUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException("userRole");

            _userRoleRepository.Insert(userRole);

            _cacheManager.RemoveByPattern(USERROLES_PATTERN_KEY);
        }

        public virtual void UpdateUserRole(UserRole userRole)
        {
            if (userRole == null)
                throw new ArgumentNullException("userRole");

            _userRoleRepository.Update(userRole);

            _cacheManager.RemoveByPattern(USERROLES_PATTERN_KEY);
        }

        #endregion

        #endregion
    }
}