using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Data.Mapping.Warehouses
{
    public class DetailLogisticUnitMap : SoftEntityTypeConfiguration<DetailLogisticUnit>
    {
        public DetailLogisticUnitMap()
        {
            HasKey(dlu => dlu.Id);
            ToTable("DetailLogisticUnit");
            Property(dlu => dlu.DateOfControl).IsRequired().HasColumnType("datetime2");

            HasRequired(dlu => dlu.LogisticUnit)
                .WithMany(dlu => dlu.DetailLogisticUnits)
                .HasForeignKey(dlu => dlu.LogisticUnitId);

            HasRequired(dlu => dlu.Product)
                .WithMany()
                .HasForeignKey(lu => lu.ProductId);

        }
    }
}