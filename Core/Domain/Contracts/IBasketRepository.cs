using Domain.Entites.BasketModule;

namespace Domain.Contracts
{
    public interface IBasketRepository
    {
        //geting basket by id
        Task<CustomerBasket?> GetBasketAsync(string id);


        //create or update basket
        Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive= null);

        //delete basket by id
        Task<bool> DeleteBasketAsync(string id);
    }
}
