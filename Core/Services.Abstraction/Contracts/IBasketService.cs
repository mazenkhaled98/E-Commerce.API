using Shared.Dtos.BasketModule;

namespace Services.Abstraction.Contracts
{
    public interface IBasketService
    {
        //get
        Task<BasketDto> GetBasketAsync(string basketId);
        //delete
        Task<bool> DeleteBasketAsync(string basketId);
        //createorupdate
        Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto);
    }

}
