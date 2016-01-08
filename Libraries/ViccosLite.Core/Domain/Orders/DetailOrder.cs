using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Core.Domain.Orders
{
    public class DetailOrder : BaseEntity
    {
        public int OrderId { get; set; }
        public virtual Order Order { get; set; }
        public int LogisticUnitId { get; set; }
        public virtual LogisticUnit LogisticUnit { get; set; } // Todos los productos pueden ser agrupados en unidades logisticas

        public double Quantity { get; set; }
        public int DetailOrderTypeId { get; set; }
        public DetailOrderType DetailOrderType //Es entregado o no es entregado
        {
            get { return (DetailOrderType)DetailOrderTypeId; }
            set { DetailOrderTypeId = (int)value; }
        }
        public string Observation { get; set; }
        public string ChainOfResponsibilities { get; set; }
        //xml del flujo si en caso no es aceptado el item todo preguntar a david esto
        //todo que pasa si no todos los productos son entregados o son devueltos ???

        public string LastUserName { get; set; }
    }
}