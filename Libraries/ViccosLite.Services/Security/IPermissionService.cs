using System.Collections.Generic;
using ViccosLite.Core.Domain.Security;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Services.Security
{
    public interface IPermissionService
    {
        void DeletePermissionRecord(PermissionRecord permission);
        PermissionRecord GetPermissionRecordById(int permissionId);
        PermissionRecord GetPermissionRecordBySystemName(string systemName);
        IList<PermissionRecord> GetAllPermissionRecords();
        void InsertPermissionRecord(PermissionRecord permission);
        void UpdatePermissionRecord(PermissionRecord permission);
        void InstallPermissions(IPermissionProvider permissionProvider);
        void UninstallPermissions(IPermissionProvider permissionProvider);
        bool Authorize(PermissionRecord permission);
        bool Authorize(PermissionRecord permission, User user);
        bool Authorize(string permissionRecordSystemName);
        bool Authorize(string permissionRecordSystemName, User user );
    }
}   