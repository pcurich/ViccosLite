using ViccosLite.Core.Domain.Orders;

namespace ViccosLite.Data.Mapping.Orders
{
    public class OrderMap : SoftEntityTypeConfiguration<Order>
    {
        public OrderMap()
        {
            ToTable("Order");
            HasKey(o => o.Id);
            Property(o => o.DateOfControl).IsRequired().HasColumnType("datetime2");

            Ignore(o => o.OrderStatus);

            HasOptional(o=>o.Warehouse)
                .WithMany()
                .HasForeignKey(o=>o.WarehouseId)
                .WillCascadeOnDelete(true);
        }
    }
}