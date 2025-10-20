using Domain.Contracts;

namespace Presistence
{
    internal static class SpecificationEvaluator
    {
      public static IQueryable<TEntity> CreateQuery<TEntity,Tkey>(IQueryable<TEntity> inputQuery,
          ISpecifications<TEntity, Tkey> specifications) where TEntity : BaseEntity<Tkey>
        {
            var query = inputQuery;
            // Apply criteria
            if (specifications.Criteria != null)
            {
                query = query.Where(specifications.Criteria);
            }
            // Apply includes
            if(specifications.IncludeExpressions != null && specifications.IncludeExpressions.Count>0)
            {
                foreach (var includeExpression in specifications.IncludeExpressions)
                {
                    query = query.Include(includeExpression);
                }
            }
            return query;
        }
    }
}
