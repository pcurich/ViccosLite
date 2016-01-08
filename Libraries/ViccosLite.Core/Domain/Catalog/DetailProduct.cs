namespace ViccosLite.Core.Domain.Catalog
{
    public class DetailProduct:BaseEntity
    {
        public int ProductParentId { get; set; }
        public string ProductParentName { get; set; }
        public int ProductNameOfNameDetailId { get; set; }
        public string ProductNameOfDetail { get; set; }

        public decimal PriceCost { get; set; }//solo para efectos informativos
        public decimal MinimumPriceSale { get; set; }//solo para efectos informativos
        public decimal MediumPriceSale { get; set; }//solo para efectos informativos
        public decimal HighPriceSale { get; set; }//solo para efectos informativos
        public int PriceTypeId { get; set; }//solo para efectos informativos
        public PriceType PriceType//solo para efectos informativos
        {
            get { return (PriceType)PriceTypeId; }
            set { PriceTypeId = (int)value; }
        }
        public double Quantity{ get; set; } 
    }
}