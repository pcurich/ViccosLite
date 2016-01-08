using ViccosLite.Core.Domain.Catalog;
using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class SupplierMap: SoftEntityTypeConfiguration<Supplier>
    {
        public SupplierMap()
        {
            ToTable("Supplier");
            HasKey(p => p.Id);

        }
    }
}