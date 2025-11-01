using AutoMapper;
using Domain.Contracts;
using Domain.Entites.IdentityModule;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using Services.Abstraction.Contracts;
using Services.Contracts;
using Services.Implementaitons;
using Shared.Common;

namespace Services.Implementations
{
    public class ServiceManager(IUnitOfWork _unitOfWork ,IMapper _mapper, IBasketRepository _basketRepository ,UserManager<User> _userManager ,IOptions<JwtOptions> options) : IServiceManager
    {

        private readonly Lazy<IProductService> _productService= new Lazy<IProductService>(()=> new ProductService(_unitOfWork, _mapper));

        private readonly Lazy<IBasketService> _BasketService = new Lazy<IBasketService>(() => new BasketService(_basketRepository, _mapper));

        private readonly Lazy<IAuthenticationService> _authenticationService = new Lazy<IAuthenticationService>(() =>new AuthenticationService(_userManager, options));


        public IProductService ProductService => _productService.Value;

        public IBasketService BasketService => _BasketService.Value;

        public IAuthenticationService authenticationService => _authenticationService.Value;
    }
}
