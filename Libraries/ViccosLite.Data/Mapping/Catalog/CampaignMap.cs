using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class CampaignMap : SoftEntityTypeConfiguration<Campaign>
    {
        public CampaignMap()
        {
            ToTable("Campaign");
            HasKey(c => c.Id);

            Property(c => c.Name).IsRequired();
            Property(c => c.Name).HasMaxLength(80);
        }
    }
}