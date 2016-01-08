using System;

namespace ViccosLite.Core.Domain.Stores
{
    public class ConfigStore:BaseEntity
    {
 
        public double TipoCambio { get; set; }
        public double ValorIgv { get; set; }
        public int LastBoleta { get; set; }
        public int LastFactura { get; set; }
        public int LsstTicket { get; set; }
        public int LastTicketFactura { get; set; }
        public int LastDegustacion { get; set; }
        //public string PatchBases { get; set; }
        //public string PatchReportes { get; set; }
        //public DateTime UltimoKardexTienda { get; set; }
        //public DateTime UltimoKardexAlmacen { get; set; }
        //public DateTime UltimoKardexGeneral { get; set; }
        //public DateTime UltimoReporte { get; set; }
        //public DateTime UltimaMigracion { get; set; }
        //http://www.youtube.com/watch?v=A0pzbxqAsyw
    }
}