using ViccosLite.Core.Domain.Stores;

namespace ViccosLite.Core.Domain.Catalog
{
    public class ProductByStore : BaseEntity
    {
        #region Keys

        public int ProductId { get; set; }
        public Product Product { get; set; }
        public int StoreId { get; set; }
        public Store Store { get; set; }

        #endregion

        public string Sku { get; protected set; }

        #region Price
        
        public decimal PriceCost { get; set; }
        public decimal MinimumPriceSale { get; set; }
        public decimal MediumPriceSale { get; set; }
        public decimal HighPriceSale { get; set; }
        public decimal CustomPriceSale { get; set; }

        public int PriceTypeId { get; set; }

        public PriceType PriceType
        {
            get { return (PriceType)PriceTypeId; }
            set { PriceTypeId = (int)value; }
        }
        
        #endregion

        #region Quantity
        public double QuantityInStore { get; set; }
        public double MinimunQuantityInStore { get; set; }

        #endregion

    }
}