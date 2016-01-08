using ViccosLite.Core.Domain.Security;

namespace ViccosLite.Data.Mapping.Security
{
    public class AclRecordMap : SoftEntityTypeConfiguration<AclRecord>
    {
        public AclRecordMap()
        {
            ToTable("AclRecord");
            HasKey(ar => ar.Id);
            Property(ar => ar.DateOfControl).IsRequired().HasColumnType("datetime2");
            Property(ar => ar.EntityName).IsRequired().HasMaxLength(400);

            HasRequired(ar => ar.UserRole)
                .WithMany()
                .HasForeignKey(ar => ar.UserRoleId)
                .WillCascadeOnDelete(true);

        }
    }
}