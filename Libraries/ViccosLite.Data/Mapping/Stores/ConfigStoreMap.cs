using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Data.Mapping.Stores
{
    public class ConfigStoreMap:SoftEntityTypeConfiguration<ConfigStore>
    {
        public ConfigStoreMap()
        {
            ToTable("ConfigStore");
            HasKey(p => p.Id);
            Property(u => u.DateOfControl).IsRequired().HasColumnType("datetime2");
        }
    }
}