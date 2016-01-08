using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Core.Domain.Security
{
    public class AclRecord:BaseEntity
    {
        public int EntityId { get; set; }
        public string EntityName { get; set; }
        public int UserRoleId { get; set; }
        public virtual UserRole UserRole { get; set; }
    }
}