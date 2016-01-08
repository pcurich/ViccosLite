using System.Linq;
using ViccosLite.Core.Domain.Orders;

namespace ViccosLite.Data.Mapping.Orders
{
    public class DetailOrderMap : SoftEntityTypeConfiguration<DetailOrder>
    {
        public DetailOrderMap()
        {
            ToTable("DetailOrder");
            HasKey(lu => lu.Id);
            Property(lu => lu.DateOfControl).IsRequired().HasColumnType("datetime2");

            HasRequired(lu => lu.Order)
                .WithMany(lu => lu.DetailOrders)
                .HasForeignKey(lu => lu.OrderId);

            HasRequired(lu => lu.LogisticUnit)
                .WithMany()
                .HasForeignKey(lu => lu.LogisticUnitId);

            Ignore(lu => lu.DetailOrderType);
        }
    }
}