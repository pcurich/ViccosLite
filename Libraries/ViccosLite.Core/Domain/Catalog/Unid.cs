using System.Collections.Generic;

namespace ViccosLite.Core.Domain.Catalog
{
    public class Unid:BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Codigo { get; set; }
        public virtual List<Product> Products { get; set; }
    }
}