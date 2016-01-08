using ViccosLite.Core.Domain.Sales;

namespace ViccosLite.Data.Mapping.Sales
{
    public class CloseCashMap : SoftEntityTypeConfiguration<CloseCash>
    {
        public CloseCashMap()
        {
            ToTable("CloseCash");
            HasKey(c => c.Id);
            Property(p => p.DateOfControl).IsRequired().HasColumnType("datetime2");

            Property(p => p.TotalSalesWithoutDiscount).HasPrecision(18, 4);
            Property(p => p.TotalDiscount).HasPrecision(18, 4);
            Property(p => p.TotalSalesWithDiscount).HasPrecision(18, 4);
            Property(p => p.DollarPaymentsSale).HasPrecision(18, 4);
            Property(p => p.PaymentCardSale).HasPrecision(18, 4);
            Property(p => p.TotalCash).HasPrecision(18, 4);
            Property(p => p.TotalTicketSoles).HasPrecision(18, 4);
            Property(p => p.TotalTicketDollar).HasPrecision(18, 4);
            Property(p => p.TotalTicketInvoiceSoles).HasPrecision(18, 4);
            Property(p => p.TotalTicketInvoiceDollar).HasPrecision(18, 4);
            Property(p => p.TotalBillSoles).HasPrecision(18, 4);
            Property(p => p.TotalBillDollar).HasPrecision(18, 4);
            Property(p => p.TotalInvoiceSoles).HasPrecision(18, 4);
            Property(p => p.TotalInvoiceDollar).HasPrecision(18, 4);

            Property(p => p.DateOfCloseShop).IsRequired().HasColumnType("datetime2");

            HasRequired(p => p.User)
                .WithMany()
                .HasForeignKey(p => p.UserId)
                .WillCascadeOnDelete(true);
        }
    }
}