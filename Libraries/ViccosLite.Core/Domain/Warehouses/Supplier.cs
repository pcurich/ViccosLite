using System.Collections.Generic;

namespace ViccosLite.Core.Domain.Warehouses
{
    public class Supplier:BaseEntity
    {
        public string RazonSocial { get; set; }
        public string Direccion { get; set; }
        public string Ruc { get; set; }
        public string Telefono { get; set; }
        public string Contacto { get; set; }
        public string EmailForOrderOnline { get; set; }
        public string HostOnLineForOrderOnline { get; set; }

        public bool SendPreOrden { get; set; }//Depende del limite de la tienda
        public virtual List<LogisticUnit> LogisticUnits { get; set; }

    }
}