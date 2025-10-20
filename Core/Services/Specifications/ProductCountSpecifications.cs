using Domain.Entites.ProductModule;
using Shared;
using System.Linq.Expressions;

namespace Services.Specifications
{
    internal class ProductCountSpecifications : BaseSpecifications<Product, int>
    {
        public ProductCountSpecifications(ProductSpecificationsParameters parameters)
            : base(p =>
                (!parameters.TypeId.HasValue || p.TypeId == parameters.TypeId)
                 &&
                (!parameters.BrandId.HasValue || p.BrandId == parameters.BrandId)
                &&
                (string.IsNullOrEmpty(parameters.search) || p.Name.ToLower().Contains(parameters.search.ToLower()))
            )
        {

        }
    }
}
