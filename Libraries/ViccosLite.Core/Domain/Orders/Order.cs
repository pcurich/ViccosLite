using System;
using System.Collections.Generic;
using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Core.Domain.Orders
{
    public class Order : BaseEntity
    {
        public Guid NumberOrder { get; set; }
        public string NumberOrder2 { get; set; } // pregnutar a david esto quien sera que genera la guia
        public string UserNameCreated { get; set; }
        public DateTime DateOfCreated { get; set; }
        public string UserNameSended { get; set; }
        public DateTime? DateOfSended { get; set; }
        public string UserNameReception { get; set; }
        public DateTime? DateOfReception { get; set; }
        public int OrderStatusId { get; set; }
        public OrderStatus OrderStatus
        {
            get { return (OrderStatus)OrderStatusId; }
            set { OrderStatusId = (int)value; }
        }
        public virtual List<DetailOrder> DetailOrders { get; set; }

        //una orden cuando es recepcionado es coloada en un almacen para su descarge
        public int? WarehouseId { get; set; }
        public virtual Warehouse Warehouse { get; set; }
    }
}