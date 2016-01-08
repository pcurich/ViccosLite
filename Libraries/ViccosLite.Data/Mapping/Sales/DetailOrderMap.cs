using ViccosLite.Core.Domain.Sales;

namespace ViccosLite.Data.Mapping.Sales
{
    public class DetailOrderMap : SoftEntityTypeConfiguration<DetailOrder>
    {
        public DetailOrderMap()
        {
            ToTable("DetailOrder");
            HasKey(p => p.Id);

            Property(p => p.DateTime).IsRequired().HasColumnType("datetime2");
            Property(p => p.PriceUnit).HasPrecision(18, 4);
            Property(p => p.Subtotal).HasPrecision(18, 4);
            Property(p => p.IgvOfSubtotal).HasPrecision(18, 4);

            HasRequired(p => p.Order)
               .WithMany(p => p.DetailOrders)
               .HasForeignKey(p => p.OrderId);

            HasRequired(p => p.ProductByStore)
               .WithMany(p => p.DetailOrders)
               .HasForeignKey(p => p.ProductByStoreId);
        }
    }
}