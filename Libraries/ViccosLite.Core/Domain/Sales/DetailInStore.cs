using ViccosLite.Core.Domain.Catalog;

namespace ViccosLite.Core.Domain.Sales
{
    public class DetailInStore : BaseEntity
    {
        public int InStoreId { get; set; }
        public InStore InStore { get; set; }
        public int ProductByStoreId { get; set; }
        public ProductByStore ProductByStore { get; set; }
        public double Quantity { get; set; }
    }
}