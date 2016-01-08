using System;
using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Core.Domain.Sales
{
    public class DetailOrder : BaseEntity
    {
        public DateTime DateTime { get; set; }
        public int Ticket { get; set; }
        public int TicketInvoice { get; set; }
        public int Boleta { get; set; }
        public int Invoice { get; set; }
        public int Degustation { get; set; }
        public double Quantity { get; set; }
        public decimal PriceUnit { get; set; }
        public decimal Subtotal { get; set; }
        public decimal IgvOfSubtotal { get; set; }

        public int OrderId { get; set; }
        public Order Order { get; set; }

        public int ProductByStoreId { get; set; }
        public ProductByStore ProductByStore { get; set; }
        public string ProductName { get; set; }
    }
}