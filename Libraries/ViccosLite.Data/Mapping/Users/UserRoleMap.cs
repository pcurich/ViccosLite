using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Data.Mapping.Users
{
    public class UserRoleMap : SoftEntityTypeConfiguration<UserRole>
    {
        public UserRoleMap()
        {
            ToTable("UserRole");
            HasKey(ur => ur.Id);
            Property(ur => ur.DateOfControl).IsRequired().HasColumnType("datetime2");

        }
    }
}