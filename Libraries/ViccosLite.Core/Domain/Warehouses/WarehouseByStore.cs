using System.Collections.Generic;
using ViccosLite.Core.Domain.Catalog;
using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Core.Domain.Warehouses
{
    public class WarehouseByStore : BaseEntity
    {
        #region Keys

        public virtual Warehouse Warehouse { get; set; }
        public int WarehouseId { get; set; }
        public virtual Store Store { get; set; }
        public int? StoreId { get; set; }

        #endregion

        


    }
}