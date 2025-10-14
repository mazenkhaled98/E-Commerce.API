using AutoMapper;
using Domain.Contracts;
using Domain.Entites.ProductModule;
using Services.Contracts;
using Shared.Dtos;

namespace Services.Implementaitons
{
    public class ProductService(IUnitOfWork _unitOfWork ,IMapper _mapper) : IProductService
    {
        public async Task<IEnumerable<BrandResultDto>> GetAllBrandsAsync()
        {
            var brandRepository = _unitOfWork.GetRepository<ProductBrand, int>();
            //1] unitofwork => genericrepository => getallbrands => Ienumerable<brand>
            var brands=await brandRepository.GetAllAsync();


            //mapping  Ienumerable<brand> => Ienumerable<BrandResultDto> [automapper]
           return _mapper.Map<IEnumerable<BrandResultDto>>(brands);
           
        }

        public async Task<IEnumerable<ProductResultDto>> GetAllProductsAsync()
        {
            var products= await _unitOfWork.GetRepository<Product, int>().GetAllAsync();

            return _mapper.Map<IEnumerable<ProductResultDto>>(products);
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types=await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeResultDto>>(types);
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
           var product=await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(id);
            return _mapper.Map<ProductResultDto?>(product);
        }
    }
}
