

using Domain.Contracts;
using Domain.Entites.ProductModule;
using System.Formats.Tar;
using System.Linq.Expressions;

namespace Services.Specifications
{
    internal abstract class BaseSpecifications<TEntity, TKey> : ISpecifications<TEntity, TKey> where TEntity : BaseEntity<TKey>
    {

        #region Criteria
        protected BaseSpecifications(Expression<Func<TEntity, bool>>? criteria)
        {
            Criteria = criteria;

        }
        public Expression<Func<TEntity, bool>>? Criteria { get; private set; } 
        #endregion

        #region Iclude
        public List<Expression<Func<TEntity, object>>> IncludeExpressions { get; } = new();



        protected void AddInclude(Expression<Func<TEntity, object>> includeExpression)
        {

            IncludeExpressions.Add(includeExpression);
        } 
        #endregion

        #region sorting
        public Expression<Func<TEntity, object>> OrderBy { get; private set; } 

        public Expression<Func<TEntity, object>> OrderByDescending { get; private set; }

   

        protected void AddOrderBy(Expression<Func<TEntity, object>> orderByExpression)
        {
            OrderBy = orderByExpression;
        }
        protected void AddOrderByDescending(Expression<Func<TEntity, object>> orderByDescExpression)
        {
            OrderByDescending = orderByDescExpression;
        }
        #endregion

        #region Pagination [skip - take]
        public int Skip { get; private set; }

        public int Take { get; private set; }

        public bool IsPaginated { get; private set; }

        protected void ApplyPagination(int pageSize,int pageIndex)
        {
            Skip = (pageIndex - 1) * pageSize;
            Take = pageSize;
            IsPaginated = true;
        }

        #endregion
    }
}
