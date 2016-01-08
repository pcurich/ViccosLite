using ViccosLite.Core.Domain.Sales;

namespace ViccosLite.Data.Mapping.Sales
{
    public class SaleMap : SoftEntityTypeConfiguration<Sale>
    {
        public SaleMap()
        {
            ToTable("Sale");
            HasKey(p => p.Id);
            Property(p => p.KeyControlId).IsRequired();
            Property(p => p.UserId).IsRequired();
            Property(p => p.UserName).IsRequired();
            Property(p => p.DateOfControl).IsRequired().HasColumnType("datetime2");

            Property(p => p.DateOfSale).IsRequired().HasColumnType("datetime2");
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

            Ignore(p => p.PaymentMethod);
            Ignore(p => p.VoucherType);

            HasOptional(tp => tp.Customer)
                .WithMany()
                .HasForeignKey(tp => tp.CustomerId)
                .WillCascadeOnDelete(true);

            HasRequired(dlu => dlu.Store)
                .WithMany()
                .HasForeignKey(lu => lu.StoreId);
        }
    }
}