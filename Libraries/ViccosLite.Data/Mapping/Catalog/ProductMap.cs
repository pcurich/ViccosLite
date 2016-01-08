using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class ProductMap : SoftEntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Product");
            HasKey(p => p.Id);
            Property(p => p.DateOfControl).IsRequired().HasColumnType("datetime2");

            Property(p => p.ProductName).IsRequired();
            Property(p => p.ProductName).HasMaxLength(50);
            Property(p => p.PriceCost).HasPrecision(18, 4);
            Property(p => p.MinimumPriceSale).HasPrecision(18, 4);
            Property(p => p.MediumPriceSale).HasPrecision(18, 4);
            Property(p => p.HighPriceSale).HasPrecision(18, 4);

            Ignore(p => p.PriceType);
            Ignore(p => p.ProductType);

            HasRequired(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId);
        }
    }
}