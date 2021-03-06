﻿using System.Collections.Generic;
using ViccosLite.Core.Domain.Warehouses;

namespace ViccosLite.Core.Domain.Catalog
{
    public class Product : BaseEntity
    {
        public string ProductName { get; set; }
        public string Description { get; set; }
        public decimal PriceCost{ get; set; }
        public decimal MinimumPriceSale { get; set; }
        public decimal MediumPriceSale { get; set; }
        public decimal HighPriceSale { get; set; }
        public int PriceTypeId { get; set; }
        public PriceType PriceType
        {
            get { return (PriceType)PriceTypeId; }
            set { PriceTypeId = (int)value; }
        }
        public string Code { get; set; } //Sku
        public int ProductTypeId { get; set; }
        public ProductType ProductType
        {
            get { return (ProductType)ProductTypeId; }
            set { ProductTypeId = (int)value; }
        }
        public int CategoryId { get; set; }
        public virtual Category Category { get; set; }

        public double TotalWarehouse { get; set; } //En todos los almacenes
        public double TotalStore { get; set; } //En todas las tiendas
        public double TotalQuantity { get; set; }

        public virtual List<DetailProduct> DetailProducts { get; set; }

        public virtual List<ProductByStore> ProductByStores { get; set; }
        
    }
}