using AutoMapper;
using Domain.Contracts;
using Domain.Entites.BasketModule;
using Domain.Exceptions;
using Services.Abstraction.Contracts;
using Shared.Dtos.BasketModule;

namespace Services.Implementations
{
    internal class BasketService(IBasketRepository _basketRepository ,IMapper _mapper) : IBasketService
    {
        public async Task<BasketDto> CreateOrUpdateBasketAsync(BasketDto basketDto)
        {
            var basket = _mapper.Map<CustomerBasket>(basketDto);
            var createdBasket=  await _basketRepository.CreateOrUpdateBasketAsync(basket);
            return createdBasket is null ? throw new Exception("cannot create the basket") : _mapper.Map<BasketDto>(createdBasket);

        }

        public async Task<bool> DeleteBasketAsync(string basketId)
        {
           return await _basketRepository.DeleteBasketAsync(basketId);
        }

        public async Task<BasketDto> GetBasketAsync(string basketId)
        {
          var basket =await _basketRepository.GetBasketAsync(basketId);
          return basket is null ?throw new BasketNotFoundException(basketId) : _mapper.Map<BasketDto>(basket);

        }
    }
}
