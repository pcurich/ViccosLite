using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Data.Mapping.Warehouses
{
    public class WarehouseByStoreMap : SoftEntityTypeConfiguration<WarehouseByStore>
    {
        public WarehouseByStoreMap()
        {
            ToTable("WarehouseByStore");
            HasKey(p => p.Id);

            HasRequired(p => p.Warehouse)
                .WithMany(p => p.WarehouseByStores)
                .HasForeignKey(p => p.WarehouseId);

            HasRequired(p => p.Store)
                .WithMany(p => p.WarehouseByStores)
                .HasForeignKey(p => p.StoreId);
        }
    }
}