using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Data.Mapping.Stores
{
    public class CompanyMap : SoftEntityTypeConfiguration<Company>
    {
        public CompanyMap()
        {
            ToTable("Company");
            HasKey(p => p.Id);
            Property(u => u.DateOfControl).IsRequired().HasColumnType("datetime2");
        }
    }
}