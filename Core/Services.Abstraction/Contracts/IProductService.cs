using Shared.Dtos;
using Shared.Enums;

namespace Services.Contracts
{
    public interface IProductService
    {
        //getallproducts
        Task<IEnumerable<ProductResultDto>> GetAllProductsAsync(int? typeId ,int? brandId,ProductSortingOptions sort);
        //getallbrands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        //getalltypes
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        //getproductbyid
        Task<ProductResultDto?> GetProductByIdAsync(int id);

    }
}
