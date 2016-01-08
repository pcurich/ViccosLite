using System;

namespace ViccosLite.Core.Domain.Stores
{
    public class ConfigStore:BaseEntity
    {
        public double ExchangeRate { get; set; }
        public double ValueIgv { get; set; }
        public int LastBill { get; set; }
        public int LastInvoice { get; set; }
        public int LastTicket { get; set; }
        public int LastTicketInvoice { get; set; }
        public int LastGift { get; set; }

        //http://www.youtube.com/watch?v=A0pzbxqAsyw
    }
}