using Domain.Entites.ProductModule;
using Shared;
using Shared.Enums;
using System.Linq.Expressions;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product, int>
    {

        //get products with their brands and types
        public ProductWithBrandAndTypeSpecifications(ProductSpecificationsParameters parameters) : 
            base(p=>
                (!parameters.TypeId.HasValue || p.TypeId== parameters.TypeId) 
                 &&
                (!parameters.BrandId.HasValue || p.BrandId== parameters.BrandId)
            )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

            //switch case for sorting
            switch (parameters.sort)
            {
                case ProductSortingOptions.PriceAsc:
                    AddOrderBy(p => p.Price);
                    break;
                case ProductSortingOptions.PriceDesc:
                    AddOrderByDescending(p => p.Price);
                    break;
                case ProductSortingOptions.NameAsc:
                    AddOrderBy(p => p.Name);
                    break;
                case ProductSortingOptions.NameDesc:
                    AddOrderByDescending(p => p.Name);
                    break;
                default:
                    break;
            }
        }


        public ProductWithBrandAndTypeSpecifications(int id) : base(p=>p.Id==id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
