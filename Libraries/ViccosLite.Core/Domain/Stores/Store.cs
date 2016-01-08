using System.Collections.Generic;
using ViccosLite.Core.Domain.Catalog;
using ViccosLite.Core.Domain.Users;
using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Core.Domain.Stores
{
    public class Store : BaseEntity
    {
        public string NameStore { get; set; }
        public string Ruc { get; set; }
        public string Phone { get; set; }
        public string Departamento { get; set; }
        public string Provincia { get; set; }
        public string Distrito { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string PrinterSeries { get; set; }
        public virtual int CompanyId { get; set; }
        public virtual Company Company { get; set; }
        public int ConfigStoreId { get; set; }
        public virtual ConfigStore ConfigStore { get; set; }
        public virtual List<User> Users { get; set; }
        public virtual List<WarehouseByStore> WarehouseByStores { get; set; }
        public virtual List<ProductByStore> ProductByStores { get; set; }

        /// <summary>
        ///     Lista separada por comas de los posibles valores de HTTP_HOST
        /// </summary>
        public string Hosts { get; set; }

        public string Url { get; set; }
        public string SecureUrl { get; set; }
        public bool SslEnabled { get; set; }
        public int DisplayOrder { get; set; }
    }
}