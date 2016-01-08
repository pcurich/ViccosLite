using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class UnidMap : SoftEntityTypeConfiguration<Unid>
    {
        public UnidMap()
        {
            ToTable("Unid");
            HasKey(p => p.Id);
        }
    }
}