using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class DetailProductMap : SoftEntityTypeConfiguration<DetailProduct>
    {
        public DetailProductMap()
        {
            ToTable("DetailProduct");
            HasKey(c => c.Id);
            Property(p => p.DateOfControl).IsRequired().HasColumnType("datetime2");

            Property(p => p.ProductParentId).IsRequired();
            Property(p => p.ProductParentName).IsRequired();
            Property(p => p.ProductNameOfDetailId).IsRequired();
            Property(p => p.ProductNameOfDetail).IsRequired();

            Property(p => p.PriceCost).HasPrecision(18, 4);
            Property(p => p.MinimumPriceSale).HasPrecision(18, 4);
            Property(p => p.MediumPriceSale).HasPrecision(18, 4);
            Property(p => p.HighPriceSale).HasPrecision(18, 4);

            Ignore(p => p.PriceType);
        }
    }
}