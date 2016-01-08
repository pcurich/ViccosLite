using ViccosLite.Core.Domain.Sales;

namespace ViccosLite.Data.Mapping.Sales
{
    public class CloseShopMap : SoftEntityTypeConfiguration<CloseShop>
    {
        public CloseShopMap()
        {
            ToTable("CloseShop");
            HasKey(c => c.Id);

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
            Property(p => p.TotalBoletaSoles).HasPrecision(18, 4);
            Property(p => p.TotalBoletaDollar).HasPrecision(18, 4);
            Property(p => p.TotalInvoiceSoles).HasPrecision(18, 4);
            Property(p => p.TotalInvoiceDollar).HasPrecision(18, 4);

            Property(p => p.DateTime).IsRequired().HasColumnType("datetime2");
  
        }
    }
}