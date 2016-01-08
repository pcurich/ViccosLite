using System;

namespace ViccosLite.Core.Domain.Sales
{
    public class DetailSale : BaseEntity
    {
        public int KeyControlId { get; set; } //este va a remplazar el SaleId en la base de datos
        public DateTime DateOfSale { get; set; }
        public int Ticket { get; set; }
        public int TicketInvoice { get; set; }
        public int Bill { get; set; }
        public int Invoice { get; set; }
        public int Gift { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public int PriceTypeId { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal IgvOfSubtotal { get; set; }
        public decimal SubTotal { get; set; }
    }
}