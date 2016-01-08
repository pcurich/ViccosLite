using System;
using ViccosLite.Core.Domain.Users;

namespace ViccosLite.Core.Domain.Sales
{
    public class CloseCash : BaseEntity
    {
        public int UserId { get; set; }
        public User User { get; set; }

        public DateTime DateOfCloseShop { get; set; }

        public decimal TotalSalesWithoutDiscount { get; set; }
        public decimal TotalDiscount { get; set; }
        public decimal TotalSalesWithDiscount { get; set; }

        public decimal DollarPaymentsSale { get; set; }
        public decimal PaymentCardSale { get; set; }
        public double QuantityPaymentCardSale { get; set; }
        public decimal TotalCash { get; set; }
        public string Result { get; set; }
        public string Print { get; set; }
        public double ExchangeRate { get; set; }

        #region Ticket

        public int QuantityOfTicket { get; set; }
        public int QuantityOfTicketCancelled { get; set; }
        public string StartTicket { get; set; }
        public string EndTicket { get; set; }
        public decimal TotalTicketSoles { get; set; }
        public decimal TotalTicketDollar { get; set; }

        #endregion

        #region TicketInvoice

        public int QuantityOfTicketInvoice { get; set; }
        public int QuantityOfTicketInvoiceCancelled { get; set; }
        public string StartTicketInvoice { get; set; }
        public string EndTicketInvoice { get; set; }
        public decimal TotalTicketInvoiceSoles { get; set; }
        public decimal TotalTicketInvoiceDollar { get; set; }

        #endregion

        #region Bill

        public int QuantityOfBill { get; set; }
        public int QuantityOfBillCancelled { get; set; }
        public string StartBill { get; set; }
        public string EndBill { get; set; }
        public decimal TotalBillSoles { get; set; }
        public decimal TotalBillDollar { get; set; }

        #endregion

        #region Invoice

        public int QuantityOfInvoice { get; set; }
        public int QuantityOfInvoiceCancelled { get; set; }
        public string StartInvoice { get; set; }
        public string EndInvoice { get; set; }
        public decimal TotalInvoiceSoles { get; set; }
        public decimal TotalInvoiceDollar { get; set; }

        #endregion
    }
}