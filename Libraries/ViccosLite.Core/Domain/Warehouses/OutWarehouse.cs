using System;

namespace ViccosLite.Core.Domain.Warehouses
{
    public class OutWarehouse:BaseEntity
    {
        public int WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }

        public int StoreId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfOut { get; set; }
    }
}