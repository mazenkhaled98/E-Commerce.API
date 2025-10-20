using Shared.Enums;

namespace Shared
{
    public class ProductSpecificationsParameters
    {
        public int? TypeId { get; set; }

        public int? BrandId { get; set; }

        public ProductSortingOptions sort { get; set; }                 
    }
}
