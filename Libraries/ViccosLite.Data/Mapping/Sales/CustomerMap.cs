using ViccosLite.Core.Domain.Sales;

namespace ViccosLite.Data.Mapping.Sales
{
    public class CustomerMap : SoftEntityTypeConfiguration<Customer>
    {
        public CustomerMap()
        {
            ToTable("Customer");
            HasKey(c => c.Id);
            Property(p => p.DateOfControl).IsRequired().HasColumnType("datetime2");

            Property(c => c.BusinessName).IsRequired();
        }
    }
}