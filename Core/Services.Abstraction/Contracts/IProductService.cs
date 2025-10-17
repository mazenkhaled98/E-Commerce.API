using Shared.Dtos;

namespace Services.Contracts
{
    public interface IProductService
    {
        //getallproducts
        Task<IEnumerable<ProductResultDto>> GetAllProductsAsync();
        //getallbrands
        Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync();
        //getalltypes
        Task<IEnumerable<TypeResultDto>> GetAllTypesAsync();
        //getproductbyid
        Task<ProductResultDto?> GetProductByIdAsync(int id);

    }
}
