using Domain.Contracts;
using Services.Abstraction.Contracts;

namespace Services.Implementations
{
    public class CasheService(ICasheRepository _casheRepository) : ICasheService
    {
        public async Task<string?> GetCachedValueAsync(string key)
         => await   _casheRepository.GetAsync(key);
        

        public async Task SetCacheValueAsync(string key, object value, TimeSpan duration)
            =>await _casheRepository.SetAsync(key, value, duration);
        
    }
}
