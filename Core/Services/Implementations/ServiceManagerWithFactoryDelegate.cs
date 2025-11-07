using Services.Abstraction.Contracts;
using Services.Contracts;

namespace Services.Implementations
{
    public class ServiceManagerWithFactoryDelegate(Func<IProductService> _productFactory
     , Func<IOrderService> _orderFactory, Func<IAuthenticationService> _authFactory
     , Func<IPaymentService> _paymentFactory, Func<IBasketService> _basketFactory) : IServiceManager
    {
   
     public IProductService ProductService => _productFactory.Invoke();

     public IBasketService BasketService => _basketFactory.Invoke();


     public IAuthenticationService authenticationService => _authFactory.Invoke();


     public IOrderService OrderService => _orderFactory.Invoke();

  
     public IPaymentService PaymentService => _paymentFactory.Invoke();
    }
}
