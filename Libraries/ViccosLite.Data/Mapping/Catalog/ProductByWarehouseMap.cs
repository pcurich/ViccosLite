using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class ProductByWarehouseMap: SoftEntityTypeConfiguration<ProductByWarehouse>
    {
        public ProductByWarehouseMap()
        {
            ToTable("ProductByWarehouse");
            HasKey(p => p.Id);

            HasRequired(p => p.Warehouse)
                .WithMany(p => p.ProductByWarehouses)
                .HasForeignKey(p => p.WarehouseId);

            HasRequired(p => p.Product)
                .WithMany(p => p.ProductByWarehouses)
                .HasForeignKey(p => p.ProductId);

        }
    }
}