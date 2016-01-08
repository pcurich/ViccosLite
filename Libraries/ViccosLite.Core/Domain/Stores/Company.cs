using System.Collections.Generic;

namespace ViccosLite.Core.Domain.Stores
{
    public class Company : BaseEntity
    {
        public string NameStore { get; set; }
        public string Ruc { get; set; }
        public string Phone { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Direccion1 { get; set; }
        public string Direccion2 { get; set; }
        public string Direccion3 { get; set; }
        public virtual List<Store> Stores { get; set; }
    }
}