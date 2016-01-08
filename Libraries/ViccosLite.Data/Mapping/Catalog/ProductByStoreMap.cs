using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class ProductByStoreMap : SoftEntityTypeConfiguration<ProductByStore>
    {
        public ProductByStoreMap()
        {
            ToTable("ProductByStore");
            HasKey(p => p.Id);

            HasRequired(p => p.Store)
                .WithMany(p => p.ProductByStores)
                .HasForeignKey(p => p.StoreId);

            HasRequired(p => p.Product)
                .WithMany(p => p.ProductByStores)
                .HasForeignKey(p => p.ProductId);

            Property(p => p.PriceCost).HasPrecision(18, 4);
            Property(p => p.PriceSale).HasPrecision(18, 4);
        }
    }
}