using System;
using System.Collections.Generic;
using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Core.Domain.Sales
{
    public class Sale : BaseEntity
    {
        public int KeyControlId { get; set; } //este va a remplazar el Id en la base de datos
        public DateTime DateOfSale { get; set; }
        public int Ticket { get; set; }
        public int TicketInvoice { get; set; }
        public int Bill { get; set; }
        public int Invoice { get; set; }
        public int Gift { get; set; }
        public string TicketPrint { get; set; }
        public int VoucherTypeId { get; set; }

        public VoucherType VoucherType
        {
            get { return (VoucherType)VoucherTypeId; }
            set { VoucherTypeId = (int)value; }
        }

        public int? CustomerId { get; set; }
        public Customer Customer { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }
        public string Description { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; }
        public decimal ExchangeRate { get; set; }
        public decimal ValueIgv { get; set; }
        public decimal SubTotal { get; set; }
        public double Discount { get; set; }
        public decimal IgvOfTotal { get; set; }
        public decimal TotalUndiscounted { get; set; }
        public decimal TotalDiscount { get; set; }
        public int PaymentMethodId { get; set; }

        public PaymentMethod PaymentMethod
        {
            get { return (PaymentMethod)PaymentMethodId; }
            set { PaymentMethodId = (int)value; }
        }

        public decimal CardPayment { get; set; }
        public decimal CashPaymentSoles { get; set; }
        public decimal CashPaymentDollar { get; set; }
        public decimal Change { get; set; }
        public virtual List<DetailSale> DetailOrders { get; set; }
    }
}