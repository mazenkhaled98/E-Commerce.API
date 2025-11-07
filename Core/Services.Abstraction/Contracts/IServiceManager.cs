using Services.Contracts;

namespace Services.Abstraction.Contracts
{
    public interface IServiceManager
    {
        public IProductService ProductService { get;  }

        public IBasketService BasketService { get; }

        public IAuthenticationService authenticationService { get; }

        public IOrderService OrderService { get; }

        public IPaymentService PaymentService { get; }

        public ICasheService CasheService { get; }
    }
}
