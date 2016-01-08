using System.Collections.Generic;

namespace ViccosLite.Framework.Events
{
    public class ProductSearchEvent
    {
        public string SearchTerm { get; set; }
        public bool SearchInDescriptions { get; set; }
        public IList<int> CategoryIds { get; set; }
        public int ManufacturerId { get; set; }
    }
}