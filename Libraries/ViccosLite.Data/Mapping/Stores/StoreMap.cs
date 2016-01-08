using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Data.Mapping.Stores
{
    public class StoreMap : SoftEntityTypeConfiguration<Store>
    {
        public StoreMap()
        {
            ToTable("Store");
            HasKey(p => p.Id);
            Property(u => u.DateOfControl).IsRequired().HasColumnType("datetime2");

            Property(p => p.NameStore).HasMaxLength(40);
            Property(p => p.Address1).HasMaxLength(40);
            Property(p => p.Address2).HasMaxLength(40);
            Property(p => p.Address3).HasMaxLength(40);

            HasRequired(p => p.Company)
               .WithMany(p => p.Stores)
               .HasForeignKey(p => p.CompanyId);
        }
    }
}