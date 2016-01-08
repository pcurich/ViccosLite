using System;
using System.Collections.Generic;
using ViccosLite.Core;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Services.Users
{
    public interface IUserService
    {
        #region Usuarios

        IPagedList<User> GetAllUsers(int pageIndex, int pageSize, int[] userRoleIds = null,
            string email = null, string username = null,
            string firstName = null, string lastName = null);
        void DeleteUser(User user);
        User GetUserById(int userId);
        IList<User> GetUserByIds(int[] userId);
        User GetUserByUserName(string username);
        User GetUserByEmail(string email);
        User GetUserBySystemName(string systemName);
        void InsertUser(User user);
        void UpdateUser(User user);

        #endregion 

        #region Roles del usuario

        void DeleteUserRole(UserRole userRole);
        UserRole GetUserRoleById(int userRoleId);
        UserRole GetUserRoleBySystemName(string systemName);
        IList<UserRole> GetAllUserRoles(bool showHidden = false);
        void InsertUserRole(UserRole userRole);
        void UpdateUserRole(UserRole userRole);

        #endregion
    }
}