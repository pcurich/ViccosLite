using System;
using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Core.Domain.Warehouses
{
    public class DetailLogisticUnit:BaseEntity
    {
        #region Keys
        public int LogisticUnitId { get; set; }
        public LogisticUnit LogisticUnit { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
        #endregion

        public double Quantity { get; set; }
        public DateTime DateOfCreated { get; set; }
        public string UserName { get; set; }

        
    }
}