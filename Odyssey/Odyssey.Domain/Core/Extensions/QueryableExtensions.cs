using System.Linq.Expressions;

namespace Odyssey.Domain.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> IfWhere<T>(this IQueryable<T> query, bool condition, Expression<Func<T, bool>> predicate)
        {
            if (condition)
                return query.Where(predicate);
            return query;
        }
    }
}
