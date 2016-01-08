using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class CategoryMap: SoftEntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            ToTable("Category");
            HasKey(c => c.Id);

            Property(c => c.Name).IsRequired();
            Property(c => c.Name).HasMaxLength(80);
        }
    }
}