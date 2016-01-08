using ViccosLite.Core.Domain.Sales;

namespace ViccosLite.Data.Mapping.Sales
{
    public class DetailInStoreMap : SoftEntityTypeConfiguration<DetailInStore>
    {
        public DetailInStoreMap()
        {
            ToTable("DetailInStore");
            HasKey(c => c.Id);

            HasRequired(p => p.InStore)
                .WithMany(p => p.DetailInStores)
                .HasForeignKey(p => p.InStoreId);

            HasRequired(p => p.ProductByStore)
                .WithMany(p => p.DetailInStores)
                .HasForeignKey(p => p.ProductByStoreId);
        }
    }
}