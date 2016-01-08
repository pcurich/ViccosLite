using ViccosLite.Core.Domain.Security;

namespace ViccosLite.Data.Mapping.Security
{
    public class PermissionRecordMap : SoftEntityTypeConfiguration<PermissionRecord>
    {
        public PermissionRecordMap()
        {
            ToTable("PermissionRecord");
            HasKey(pr => pr.Id);
            Property(pr => pr.DateOfControl).IsRequired().HasColumnType("datetime2");
            Property(pr => pr.Name).IsRequired();
            Property(pr => pr.SystemName).IsRequired().HasMaxLength(255);
            Property(pr => pr.Category).IsRequired().HasMaxLength(255);

            HasMany(m => m.UserRoles)
                .WithMany()
                .Map(m => m.ToTable("PermissionRecord_UserRole_Mapping"));
        }
    }
}