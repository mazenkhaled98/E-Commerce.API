using Domain.Entites;

namespace Domain.Contracts
{
    public interface IUnitOfWork
    {
        //complete , savechangesasync

        Task<int> SaveChangesAsync();

        //2] method return obj from geric repository[TEntity]
        //new GenericRepository<Product,int>
        //new GenericRepository<Order,Guid>
        IGenericRepository<TEntity,TKey> GetRepository<TEntity,TKey>() where TEntity : BaseEntity<TKey>;
    }
}
