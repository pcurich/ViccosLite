using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class ProductMap : SoftEntityTypeConfiguration<Product>
    {
        public ProductMap()
        {
            ToTable("Product");
            HasKey(p => p.Id);

            Property(p => p.Name).IsRequired();
            Property(p => p.Name).HasMaxLength(50);
            Property(p => p.PriceCost).HasPrecision(18, 4);
            Property(p => p.PriceSale).HasPrecision(18, 4);

            HasRequired(p => p.Campaign)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CampaignId);

            HasRequired(p => p.Category)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.CategoryId);

            HasRequired(p => p.Supplier)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.SupplierId);

            HasRequired(p => p.Unid)
                .WithMany(p => p.Products)
                .HasForeignKey(p => p.UnidId);
        }
    }
}