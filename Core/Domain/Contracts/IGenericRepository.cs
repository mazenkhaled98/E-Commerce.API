using Domain.Entites;

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

        #region Specfications
        //getall
        Task<IEnumerable<TEntity>> GetAllAsync(ISpecifications<TEntity,Tkey> specifications);

        //getbyid
        Task<TEntity?> GetByIdAsync(ISpecifications<TEntity, Tkey> specifications); 

        Task<int> CountAsync(ISpecifications<TEntity, Tkey> specifications);
        #endregion
    }
}
