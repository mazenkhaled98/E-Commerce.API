namespace Services.Abstraction.Contracts
{
    public interface ICasheService
    {
        //Get
 
        Task<string?> GetCachedValueAsync(string key);
        //Set

        Task SetCacheValueAsync(string key, object value, TimeSpan duration);
    }
}
