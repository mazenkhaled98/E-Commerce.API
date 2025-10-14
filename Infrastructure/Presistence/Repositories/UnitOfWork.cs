using Domain.Contracts;
using Presistence.Data;
using System.Collections.Concurrent;

namespace Presistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly StoreDbContext _dbContext;
        private readonly ConcurrentDictionary<string, object> _repositories;

        public UnitOfWork(StoreDbContext dbContext)
        {
            _dbContext = dbContext;
            _repositories = new();
        }

        

        public IGenericRepository<TEntity, TKey> GetRepository<TEntity, TKey>() where TEntity : BaseEntity<TKey>
        => (IGenericRepository<TEntity, TKey>)
            _repositories.GetOrAdd(typeof(TEntity).Name,(_)=> new GenericRepository<TEntity, TKey>(_dbContext));
        //dictionary ==> key , value
        //key ==>name of entity [product] => string
        //value ==> obj from generic repository [new GenericRepository<Product,int>] => object  
        //dictionary("Product", new GenericRepository<Product,int>(_dbContext))

        //var key = typeof(TEntity).Name; // product => "Product"=> string

        //if (!_repositories.ContainsKey(key))
        //{
        //    _repositories[key]= new GenericRepository<TEntity, TKey>(_dbContext);
        //}
        //return (IGenericRepository<TEntity, TKey>) _repositories[key];




        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();

        }
    }
}
