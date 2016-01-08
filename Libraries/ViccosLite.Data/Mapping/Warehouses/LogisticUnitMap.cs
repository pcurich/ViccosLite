using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Data.Mapping.Warehouses
{
    public class LogisticUnitMap:SoftEntityTypeConfiguration<LogisticUnit>
    {
        public LogisticUnitMap()
        {
            ToTable("LogisticUnit");
            HasKey(lu => lu.Id);
            Property(lu => lu.DateOfControl).IsRequired().HasColumnType("datetime2");
            Property(lu => lu.DateOfCreated).IsRequired().HasColumnType("datetime2");

            HasRequired(lu => lu.Supplier)
                .WithMany(lu => lu.LogisticUnits)
                .HasForeignKey(lu => lu.SupplierId);
        }
    }
}