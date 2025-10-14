using Services.Contracts;

namespace Services.Abstraction.Contracts
{
    public interface IServiceManager
    {
        public IProductService ProductService { get;  }
    }
}
