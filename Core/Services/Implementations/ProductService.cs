using AutoMapper;
using Domain.Contracts;
using Domain.Entites.ProductModule;
using Services.Contracts;
using Services.Specifications;
using Shared;
using Shared.Dtos;
using Shared.Enums;

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

        public async Task<PaginatedResult<ProductResultDto>> GetAllProductsAsync(ProductSpecificationsParameters parameters)
        {
            var productRepository = _unitOfWork.GetRepository<Product, int>();
            var productCountSpecifications = new ProductCountSpecifications(parameters);
            var specifications = new ProductWithBrandAndTypeSpecifications(parameters);
            var products= await productRepository.GetAllAsync(specifications);
            var productResultDtos=_mapper.Map<IEnumerable<ProductResultDto>>(products);
            var pageSize = productResultDtos.Count();
            var totalCount = await productRepository.CountAsync(productCountSpecifications);
            return new  PaginatedResult<ProductResultDto>(

                parameters.PageIndex,
                pageSize,
                totalCount,
                productResultDtos
                );
        }

        public async Task<IEnumerable<TypeResultDto>> GetAllTypesAsync()
        {
            var types=await _unitOfWork.GetRepository<ProductType, int>().GetAllAsync();
            return _mapper.Map<IEnumerable<TypeResultDto>>(types);
        }

        public async Task<ProductResultDto?> GetProductByIdAsync(int id)
        {
            var specifications = new ProductWithBrandAndTypeSpecifications(id);
            var product=await _unitOfWork.GetRepository<Product, int>().GetByIdAsync(specifications);
            return _mapper.Map<ProductResultDto?>(product);
        }
    }
}
