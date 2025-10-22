using Domain.Contracts;
using Domain.Entites.BasketModule;
using StackExchange.Redis;
using System.Text.Json;

namespace Presistence.Repositories
{
    public class BasketRepository(IConnectionMultiplexer _connection) : IBasketRepository
    {
        private readonly IDatabase _database = _connection.GetDatabase();

        public async Task<CustomerBasket> CreateOrUpdateBasketAsync(CustomerBasket basket, TimeSpan? timeToLive = null)
        {
           var jsonBasket= JsonSerializer.Serialize(basket);

            //stringsetAsync(key(id),value(data),expiry)
            var result = await _database.StringSetAsync(basket.Id, jsonBasket, timeToLive?? TimeSpan.FromDays(30));
            return result ? await GetBasketAsync(basket.Id) : null;

        }

        public async Task<bool> DeleteBasketAsync(string id)
       =>await _database.KeyDeleteAsync(id);

        public async Task<CustomerBasket?> GetBasketAsync(string id)
        {
             var data =await  _database.StringGetAsync(id);
            if (data.IsNullOrEmpty)
                return null;
            return JsonSerializer.Deserialize<CustomerBasket>(data!);

        }
    }
}
