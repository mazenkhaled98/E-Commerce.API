using Shared.Enums;

namespace Shared
{
    public class ProductSpecificationsParameters
    {
        private const int defaultPageSize = 5;

        private int maxPageSize = 10;
        public int? TypeId { get; set; }

        public int? BrandId { get; set; }

        public ProductSortingOptions sort { get; set; }

        public string? search { get; set; }

        public int PageIndex { get; set; }

        private int _PageSize= defaultPageSize;

        public int PageSize
        {
            get { return _PageSize; }
            set { _PageSize = value > maxPageSize? maxPageSize:value; }
        }

    }
}
