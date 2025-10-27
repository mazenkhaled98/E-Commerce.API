using AutoMapper;
using Domain.Contracts;
using Services.Abstraction.Contracts;
using Services.Contracts;
using Services.Implementaitons;

namespace Services.Implementations
{
    public class ServiceManager(IUnitOfWork _unitOfWork ,IMapper _mapper, IBasketRepository _basketRepository) : IServiceManager
    {

        private readonly Lazy<IProductService> _productService= new Lazy<IProductService>(()=> new ProductService(_unitOfWork, _mapper));

        private readonly Lazy<IBasketService> _BasketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));
        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _BasketService.Value;
    }
}
