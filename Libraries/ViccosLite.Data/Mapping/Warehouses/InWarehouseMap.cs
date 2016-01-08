using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Data.Mapping.Warehouses
{
    public class InWarehouseMap : SoftEntityTypeConfiguration<InWarehouse>
    {
        public InWarehouseMap()
        {
            ToTable("InWarehouse");
            HasKey(p => p.Id);
            Property(p => p.DateOfControl).IsRequired().HasColumnType("datetime2");
            Property(p => p.DateOfIn).IsRequired().HasColumnType("datetime2");


            Property(tp => tp.UnitPrice).HasPrecision(18, 4);
            Property(tp => tp.SubTotal).HasPrecision(18, 4);
            Property(tp => tp.Igv).HasPrecision(18, 4);
            Property(tp => tp.Total).HasPrecision(18, 4);

        }
    }
}