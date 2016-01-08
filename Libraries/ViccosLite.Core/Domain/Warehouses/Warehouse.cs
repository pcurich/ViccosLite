using System.Collections.Generic;

namespace ViccosLite.Core.Domain.Warehouses
{
    public class Warehouse : BaseEntity
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string Departament { get; set; }
        public string Province { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }

        public string Phone { get; set; }
        public virtual List<WarehouseByStore> WarehouseByStores { get; set; }

        //Si entra a almacen viene de una orden y solo se registra esto como si fuera una evidencia de que la orden
        //ha sido ingresada al almacen
        public virtual List<InWarehouse> InWarehouses { get; set; }

        //Si sale del almacen es para que se transfiera hacia tienda, por eso hay que saber a que tienda se fue y 
        //que producto fue 
        public virtual List<OutWarehouse> OutWarehouses { get; set; } 
        
    }
}