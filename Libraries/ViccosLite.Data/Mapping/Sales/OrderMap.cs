using ViccosLite.Core.Domain.Sales;

namespace ViccosLite.Data.Mapping.Sales
{
    public class OrderMap : SoftEntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            ToTable("Order");
            HasKey(p => p.Id);

            Property(p => p.DateTime).IsRequired().HasColumnType("datetime2");
            Property(p => p.ExchangeRate).HasPrecision(18, 4);
            Property(p => p.ValueIgv).HasPrecision(18, 4);
            Property(p => p.SubTotal).HasPrecision(18, 4);
            Property(p => p.TotalUndiscounted).HasPrecision(18, 4);
            Property(p => p.TotalDiscount).HasPrecision(18, 4);
            Property(p => p.IgvOfTotal).HasPrecision(18, 4);
            Property(p => p.CardPayment).HasPrecision(18, 4);
            Property(p => p.CashPaymentSoles).HasPrecision(18, 4);
            Property(p => p.CashPaymentDollar).HasPrecision(18, 4);
            Property(p => p.Change).HasPrecision(18, 4);

            HasOptional(tp => tp.Customer)
                .WithMany()
                .HasForeignKey(tp => tp.CustomerId)
                .WillCascadeOnDelete(true);
        }
    }
}