using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Data.Mapping.Catalog
{
    public class CategoryMap: SoftEntityTypeConfiguration<Category>
    {
        public CategoryMap()
        {
            ToTable("Category");
            HasKey(c => c.Id);
            Property(p => p.DateOfControl).IsRequired().HasColumnType("datetime2");

            Property(c => c.CategoryName).IsRequired();
            Property(c => c.CategoryName).HasMaxLength(80);
        }
    }
}