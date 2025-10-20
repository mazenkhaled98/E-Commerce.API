using Domain.Entites.ProductModule;
using System.Linq.Expressions;

namespace Domain.Contracts
{
    public interface ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        //signature for prop for [expression => where]
        public Expression<Func<TEntity,bool>>? Criteria { get; }

        //signature for prop for list [expression => includes]
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; }


        //signature for prop for orderby [expression => orderby]

        public Expression<Func<TEntity, object>> OrderBy { get; }

        //signature for prop for orderbydesc [expression => orderbydesc]
        public Expression<Func<TEntity, object>> OrderByDescending { get; }
    }
}
