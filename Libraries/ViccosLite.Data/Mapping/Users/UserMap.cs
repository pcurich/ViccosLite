using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Data.Mapping.Users
{
    public class UserMap : SoftEntityTypeConfiguration<User>
    {
        public UserMap()
        {
            ToTable("User");
            HasKey(u => u.Id);
            Property(u =>u.DateOfControl).IsRequired().HasColumnType("datetime2");

            Property(u => u.UserName).HasMaxLength(1000);
            Property(u => u.Email).HasMaxLength(1000);

            HasMany(m => m.Stores)
                .WithMany()
                .Map(m => m.ToTable("User_Store_Mapping"));

            HasMany(m => m.UserRoles)
                .WithMany()
                .Map(m => m.ToTable("User_UserRoles_Mapping")); ;
        }
    }
}