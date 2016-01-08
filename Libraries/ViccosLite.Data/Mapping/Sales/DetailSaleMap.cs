using ViccosLite.Core.Domain.Sales;

namespace ViccosLite.Data.Mapping.Sales
{
    public class DetailSaleMap : SoftEntityTypeConfiguration<DetailSale>
    {
        public DetailSaleMap()
        {
            ToTable("DetailSale");
            HasKey(p => p.Id);
            Property(p => p.DateOfControl).IsRequired().HasColumnType("datetime2");
            Property(p => p.KeyControlId).IsRequired();
            Property(p => p.ProductId).IsRequired();
            Property(p => p.ProductName).IsRequired();

            Property(p => p.DateOfSale).IsRequired().HasColumnType("datetime2");
            Property(p => p.PriceUnit).HasPrecision(18, 4);
            Property(p => p.SubTotal).HasPrecision(18, 4);
            Property(p => p.IgvOfSubtotal).HasPrecision(18, 4);

        }
    }
}