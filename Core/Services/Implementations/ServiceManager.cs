using AutoMapper;
using Domain.Contracts;
using Domain.Entites.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Services.Abstraction.Contracts;
using Services.Contracts;
using Services.Implementaitons;
using Shared.Common;

namespace Services.Implementations
{
    public class ServiceManager(IUnitOfWork _unitOfWork, IMapper _mapper, IBasketRepository _basketRepository, UserManager<User> _userManager, IOptions<JwtOptions> options,IConfiguration _configuration) : IServiceManager
    {

        private readonly Lazy<IProductService> _productService= new Lazy<IProductService>(()=> new ProductService(_unitOfWork, _mapper));

        private readonly Lazy<IBasketService> _BasketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));

        private readonly Lazy<IAuthenticationService> _authenticationService = new Lazy<IAuthenticationService>(() =>new AuthenticationService(_userManager, options,_mapper));

        private readonly Lazy<IOrderService> _orderService = new Lazy<IOrderService>(() => new OrderService(_mapper,_basketRepository,_unitOfWork));

        private readonly Lazy<IPaymentService> _PaymentService = new Lazy<IPaymentService>(() => new PaymentService(_configuration,_basketRepository,_unitOfWork,_mapper  ));


        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _BasketService.Value;

        public IAuthenticationService authenticationService => _authenticationService.Value;

        public IOrderService OrderService =>_orderService.Value;

        public IPaymentService PaymentService => _PaymentService.Value;  
    }
}
