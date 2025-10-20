using Domain.Entites.ProductModule;
using System.Linq.Expressions;

namespace Services.Specifications
{
    internal class ProductWithBrandAndTypeSpecifications : BaseSpecifications<Product, int>
    {

        //get products with their brands and types
        public ProductWithBrandAndTypeSpecifications(int? typeId,int? brandId) : 
            base(p=>
                (!typeId.HasValue || p.TypeId==typeId) 
                 &&
                (!brandId.HasValue || p.BrandId==brandId)
            )
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }

        public ProductWithBrandAndTypeSpecifications(int id) : base(p=>p.Id==id)
        {
            AddInclude(p => p.ProductBrand);
            AddInclude(p => p.ProductType);
        }
    }
}
