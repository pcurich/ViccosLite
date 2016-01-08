using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Data.Mapping.Stores
{
    public class StoreMap : SoftEntityTypeConfiguration<Store>
    {
        public StoreMap()
        {
            ToTable("Store");
            HasKey(p => p.Id);

            Property(p => p.NameStore).HasMaxLength(40);
            Property(p => p.Addredd1).HasMaxLength(40);
            Property(p => p.Addredd2).HasMaxLength(40);
            Property(p => p.Addredd3).HasMaxLength(40);

            HasRequired(p => p.Company)
               .WithMany(p => p.Stores)
               .HasForeignKey(p => p.CompanyId);
        }
    }
}