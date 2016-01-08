using System.Collections.Generic;

namespace ViccosLite.Core.Domain.Stores
{
    public class Company : BaseEntity
    {
        public string StoreName { get; set; }
        public string Ruc { get; set; }
        public string Phone { get; set; }
        public string Department { get; set; }
        public string Province { get; set; }
        public string District { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public virtual List<Store> Stores { get; set; }
    }
}