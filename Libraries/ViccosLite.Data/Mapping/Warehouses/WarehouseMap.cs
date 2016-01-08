using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Data.Mapping.Warehouses
{
    public class WarehouseMap:SoftEntityTypeConfiguration<Warehouse>
    {
        public WarehouseMap()
        {
            ToTable("Warehouse");
            HasKey(p => p.Id);
            Property(u => u.DateOfControl).IsRequired().HasColumnType("datetime2");


        }
    }
}