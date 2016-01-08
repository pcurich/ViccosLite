using ViccosLite.Core.Domain.Sales;

namespace ViccosLite.Data.Mapping.Sales
{
    public class InStoreMap : SoftEntityTypeConfiguration<InStore>
    {
        public InStoreMap()
        {
            ToTable("InStore");
            HasKey(p => p.Id);

            Property(p => p.DateTime).IsRequired().HasColumnType("datetime2");
        }
    }
}