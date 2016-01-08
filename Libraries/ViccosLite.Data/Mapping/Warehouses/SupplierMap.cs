using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Data.Mapping.Warehouses
{
    public class SupplierMap : SoftEntityTypeConfiguration<Supplier>
    {
        public SupplierMap()
        {
            ToTable("Supplier");
            HasKey(lu => lu.Id);
            Property(lu => lu.DateOfControl).IsRequired().HasColumnType("datetime2");
        }
    }
}