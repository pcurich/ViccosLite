using System.Collections.Generic;
using ViccosLite.Core.Domain.Security;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Services.Security
{
    public class DefaultPermissionRecord
    {
        public DefaultPermissionRecord()
        {
            PermissionRecords = new List<PermissionRecord>();
        }

        public string CustomerRoleSystemName { get; set; }
        public IEnumerable<PermissionRecord> PermissionRecords { get; set; }
    }
}