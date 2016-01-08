using System;
using System.Collections.Generic;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Core.Domain.Sales
{
    public class Order : BaseEntity
    {
        public DateTime DateTime { get; set; }
        public int Ticket { get; set; }
        public int TicketInvoice { get; set; }
        public int Boleta { get; set; }
        public int Invoice { get; set; }
        public int Degustation { get; set; }
        public bool IsDegustation { get; set; }
        public string TicketPrint { get; set; }

        public int VoucherTypeId { get; set; }
        public VoucherType VoucherType
        {
            get { return (VoucherType) VoucherTypeId; }
            set {VoucherTypeId = (int) value; } 
        }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }

        public string Description { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public User User { get; set; }

        public decimal ExchangeRate { get; set; }
        public decimal ValueIgv { get; set; }
        public decimal SubTotal { get; set; }
        public double Discount { get; set; }
        public decimal TotalUndiscounted { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal IgvOfTotal { get; set; }
        public decimal CardPayment { get; set; }
        public decimal CashPaymentSoles { get; set; }
        public decimal CashPaymentDollar { get; set; }
        public decimal Change { get; set; }
        public int PaymentMethodId { get; set; }

        public PaymentMethod PaymentMethod
        {
            get { return (PaymentMethod)PaymentMethodId; }
            set { PaymentMethodId = (int)value; }
        }

        public virtual List<DetailOrder> DetailOrders { get; set; } 
        
    }
}