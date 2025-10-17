using Domain.Entites.ProductModule;

namespace Domain.Contracts
{
    public interface IGenericRepository<TEntity,Tkey> where TEntity : BaseEntity<Tkey>
    {
        //getall
        Task<IEnumerable<TEntity>> GetAllAsync(bool asNoTracking = false);

        //getbyid
        Task<TEntity?> GetByIdAsync(Tkey id);

        //add
        Task AddAsync(TEntity entity);

        //remove
        void Delete(TEntity entity);

        //update
        void Update(TEntity entity);
    }
}
