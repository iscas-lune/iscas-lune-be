using System.Linq.Expressions;

namespace iscas_lune.Api.Infrastructure.Filtros.IQuerable;

public static class SearchFilter
{
    public static IQueryable<TEntity> WhereIsNotNull<TEntity>(this IQueryable<TEntity> querable, Expression<Func<TEntity, bool>>? where)
    {
        if (where == null)
            return querable;

        return querable.Where(where);
    }
}
