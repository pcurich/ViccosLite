using System.Collections.Generic;
using ViccosLite.Core.Domain.Security;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Services.Security
{
    public class StandardPermissionProvider : IPermissionProvider
    {
        public IEnumerable<PermissionRecord> GetPermissions()
        {
            return new[]
            {
                AccessAdminPanel
            };
        }

        public IEnumerable<DefaultPermissionRecord> GetDefaultPermissions()
        {
            throw new System.NotImplementedException();
        }

        #region Permisos para el area de admin

        public static readonly PermissionRecord AccessAdminPanel = new PermissionRecord
        {
            Name = "Access admin area",
            SystemName = "AccessAdminPanel",
            Category = "Standard"
        };

        #endregion
    }
}