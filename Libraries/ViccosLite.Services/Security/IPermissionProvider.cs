using System.Collections.Generic;
using ViccosLite.Core.Domain.Security;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Services.Security
{
    public interface IPermissionProvider
    {
        IEnumerable<PermissionRecord> GetPermissions();
        IEnumerable<DefaultPermissionRecord> GetDefaultPermissions();
    }
}