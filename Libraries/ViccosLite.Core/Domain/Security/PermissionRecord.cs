using System.Collections.Generic;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Core.Domain.Security
{
    public class PermissionRecord : BaseEntity
    {
        public string Name { get; set; }
        public string SystemName { get; set; }
        public string Category { get; set; }

        private List<UserRole> _userRoles;
        public virtual List<UserRole> UserRoles
        {
            get { return _userRoles ?? (_userRoles = new List<UserRole>()); }
            protected set { _userRoles = value; }
        }
    }
}