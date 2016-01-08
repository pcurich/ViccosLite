using System.Collections.Generic;

namespace ViccosLite.Core.Domain.Catalog
{
    public class Category : BaseEntity
    {
        public string CategoryName { get; set; }
        public string Description { get; set; }
        public bool Published { get; set; }
        public int DisplayOrder { get; set; }
        public int ParentCategoryId { get; set; }
        public virtual List<Product> Products { get; set; }
        public bool LimitedToStores { get; set; }
    }
}