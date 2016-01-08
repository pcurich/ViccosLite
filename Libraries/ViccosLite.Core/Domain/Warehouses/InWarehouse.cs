using System;

namespace ViccosLite.Core.Domain.Warehouses
{
    public class InWarehouse:BaseEntity
    {
        public int WarehouseId { get; set; } 
        public virtual Warehouse Warehouse { get; set; }

        public decimal UnitPrice { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Igv { get; set; }
        public decimal Total { get; set; }

        public int OrdenNumber { get; set; }
        public int LogisticUnitId { get; set; }
        public int ProductId { get; set; }
        public string ProductName { get; set; }
        public double Quantity { get; set; }
        public string UserName { get; set; }
        public DateTime DateOfIn { get; set; }
    }
}