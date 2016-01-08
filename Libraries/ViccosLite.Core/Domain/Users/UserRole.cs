using System.Collections.Generic;
using ViccosLite.Core.Domain.Security;

namespace ViccosLite.Core.Domain.Users
{
    public class UserRole : BaseEntity
    {
        public string Name { get; set; }

        private List<User> _users;
        public virtual List<User> Users
        {
            get { return _users ?? (_users = new List<User>()); }
            set { _users = value; }
        }

        private List<PermissionRecord> _permissionRecords;
        public virtual List<PermissionRecord> PermissionRecords
        {
            get { return _permissionRecords ?? (_permissionRecords = new List<PermissionRecord>()); }
            set { _permissionRecords = value; }
        }

        public bool IsSystemRole { get; set; }
        public string SystemName { get; set; }
    }
}