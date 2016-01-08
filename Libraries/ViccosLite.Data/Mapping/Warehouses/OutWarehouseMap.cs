using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Data.Mapping.Warehouses
{
    public class OutWarehouseMap : SoftEntityTypeConfiguration<OutWarehouse>
    {
        public OutWarehouseMap()
        {
            ToTable("OutWarehouse");
            HasKey(p => p.Id);
            Property(p => p.DateOfControl).IsRequired().HasColumnType("datetime2");
            Property(p => p.DateOfOut).IsRequired().HasColumnType("datetime2");

            HasRequired(tp => tp.Warehouse)
                .WithMany(p => p.OutWarehouses)
                .HasForeignKey(tp => tp.WarehouseId);

        }
    }
}