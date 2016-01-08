using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class DetailProductMap : SoftEntityTypeConfiguration<DetailProduct>
    {
        public DetailProductMap()
        {
            ToTable("DetailProduct");
            HasKey(c => c.Id);

            Property(p => p.PriceCost).HasPrecision(18, 4);
            Property(p => p.PriceSale).HasPrecision(18, 4);
        }
    }
}