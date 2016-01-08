using System;
using System.Collections.Generic;
using System.Linq;
using ViccosLite.Core;
using ViccosLite.Core.Caching;
using ViccosLite.Core.Data;
using ViccosLite.Core.Domain.Security;
using ViccosLite.Core.Domain.Users;
using ViccosLite.Services.Users;

namespace ViccosLite.Services.Security
{
    public class PermissionService : IPermissionService
    {
        #region Ctr

        public PermissionService(IRepository<PermissionRecord> permissionPecordRepository, IUserService customerService,
            IWorkContext workContext, ICacheManager cacheManager)
        {
            _permissionPecordRepository = permissionPecordRepository;
            _customerService = customerService;
            _workContext = workContext;
            _cacheManager = cacheManager;
        }

        #endregion

        #region Keys

        /// <summary>
        ///     Llave para el cache
        /// </summary>
        /// <remarks>
        ///     {0} : Identificador del rol del cliente
        ///     {1} : permiso del nomnre del sistem
        /// </remarks>
        private const string PERMISSIONS_ALLOWED_KEY = "Soft.permission.allowed-{0}-{1}";

        /// <summary>
        ///     Llave para borrar el cache
        /// </summary>
        private const string PERMISSIONS_PATTERN_KEY = "Soft.permission.";

        #endregion

        #region Campos

        private readonly IRepository<PermissionRecord> _permissionPecordRepository;
        private readonly IUserService _customerService;
        private readonly IWorkContext _workContext;
        private readonly ICacheManager _cacheManager;

        #endregion

        #region Util

        /// <summary>
        ///     Permiso de autorizacion
        /// </summary>
        /// <param name="permissionRecordSystemName">Nombre del permiso del sistema</param>
        /// <param name="userRole">Rol del USUARIO</param>
        /// <returns>true - autorizado; otros, false</returns>
        protected virtual bool Authorize(string permissionRecordSystemName, UserRole userRole)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var key = string.Format(PERMISSIONS_ALLOWED_KEY, userRole.Id, permissionRecordSystemName);
            return _cacheManager.Get(key, () =>
            {
                foreach (var permission1 in userRole.PermissionRecords)
                    if (permission1.SystemName.Equals(permissionRecordSystemName,
                        StringComparison.InvariantCultureIgnoreCase))
                        return true;

                return false;
            });
        }

        #endregion

        #region Metodos

        public virtual void DeletePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionPecordRepository.Delete(permission);
            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        public virtual PermissionRecord GetPermissionRecordById(int permissionId)
        {
            return permissionId == 0
                ? null
                : _permissionPecordRepository.GetById(permissionId);
        }

        public virtual PermissionRecord GetPermissionRecordBySystemName(string systemName)
        {
            if (String.IsNullOrWhiteSpace(systemName))
                return null;

            var query = from pr in _permissionPecordRepository.Table
                        where pr.SystemName == systemName
                        orderby pr.Id
                        select pr;

            var permisionRecord = query.FirstOrDefault();
            return permisionRecord;
        }

        public virtual IList<PermissionRecord> GetAllPermissionRecords()
        {
            var query = from pr in _permissionPecordRepository.Table
                        orderby pr.Name
                        select pr;

            var permision = query.ToList();
            return permision;
        }

        public virtual void InsertPermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionPecordRepository.Insert(permission);
            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        public virtual void UpdatePermissionRecord(PermissionRecord permission)
        {
            if (permission == null)
                throw new ArgumentNullException("permission");

            _permissionPecordRepository.Update(permission);
            _cacheManager.RemoveByPattern(PERMISSIONS_PATTERN_KEY);
        }

        public void InstallPermissions(IPermissionProvider permissionProvider)
        {
            //TODO
            throw new NotImplementedException();
        }

        public void UninstallPermissions(IPermissionProvider permissionProvider)
        {
            //TODO
            throw new NotImplementedException();
        }

        public bool Authorize(PermissionRecord permission)
        {
            return Authorize(permission, _workContext.CurrentUser);
        }

        public bool Authorize(PermissionRecord permission, User user)
        {
            if (permission == null)
                return false;

            if (user == null)
                return false;

            return Authorize(permission.SystemName, user);
        }

        public bool Authorize(string permissionRecordSystemName)
        {
            return Authorize(permissionRecordSystemName, _workContext.CurrentUser);
        }

        public bool Authorize(string permissionRecordSystemName, User user)
        {
            if (String.IsNullOrEmpty(permissionRecordSystemName))
                return false;

            var customerRoles = user.UserRoles.Where(cr => cr.Active);
            foreach (var role in customerRoles)
                if (Authorize(permissionRecordSystemName, role))
                    return true; //Hay permiso

            //No se encontro permiso
            return false;
        }

        #endregion
    }
}