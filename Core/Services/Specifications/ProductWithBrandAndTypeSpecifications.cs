using Domain.Entites.ProductModule;
using Shared.Enums;
using System.Linq.Expressions;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product, int>
    {

        //get products with their brands and types
        public ProductWithBrandAndTypeSpecifications(int? typeId, int? brandId, ProductSortingOptions sort) : 
            base(p=>
                (!typeId.HasValue || p.TypeId==typeId) 
                 &&
                (!brandId.HasValue || p.BrandId==brandId)
            )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);

            //switch case for sorting
            switch (sort)
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
