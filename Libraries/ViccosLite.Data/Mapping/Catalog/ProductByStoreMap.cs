using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class ProductByStoreMap : SoftEntityTypeConfiguration<ProductByStore>
    {
        public ProductByStoreMap()
        {
            ToTable("ProductByStore");
            HasKey(p => p.Id);
            Property(p => p.DateOfControl).IsRequired().HasColumnType("datetime2");

            Property(p => p.Code).IsRequired();
            Property(p => p.PriceCost).HasPrecision(18, 4);
            Property(p => p.MinimumPriceSale).HasPrecision(18, 4);
            Property(p => p.MediumPriceSale).HasPrecision(18, 4);
            Property(p => p.HighPriceSale).HasPrecision(18, 4);

            Ignore(p => p.PriceType);

            HasRequired(p => p.Store)
                .WithMany(p => p.ProductByStores)
                .HasForeignKey(p => p.StoreId);

            HasRequired(p => p.Product)
                .WithMany(p => p.ProductByStores)
                .HasForeignKey(p => p.ProductId);
        }
    }
}