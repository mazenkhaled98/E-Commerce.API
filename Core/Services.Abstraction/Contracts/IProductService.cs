using Shared;
using Shared.Dtos.ProductModule;
using Shared.Enums;

namespace Services.Contracts
{
    public interface IProductService
    {
        //getallproducts
        Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters parameters);
        //getallbrands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        //getalltypes
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        //getproductbyid
        Task<ProductResultDto?> GetProductByIdAsync(int id);

    }
}
