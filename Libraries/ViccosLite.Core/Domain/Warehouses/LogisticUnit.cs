using System;
using System.Collections.Generic;
using ViccosLite.Core.Domain.Orders;

namespace ViccosLite.Core.Domain.Warehouses
{
    public class LogisticUnit : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public string Source { get; set; } //Dump14
        public string UserName { get; set; }
        public int SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
        public DateTime DateOfCreated { get; set; }
        public virtual List<DetailLogisticUnit> DetailLogisticUnits { get; set; }
    }
}