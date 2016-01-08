namespace ViccosLite.Core.Domain.Orders
{
    public enum OrderStatus
    {
        PreSent=10, //el sistema hace el pedido 
        Sent=20, //el sistema o el usuario envia la orden
        Processing = 30, //el proveedor acepta el pedido
        Delivered=40, //el proveedor entrega los productos
        Cancelled = 50 //la orden a sido cancelada
    }
}