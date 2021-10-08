using System.Linq;

namespace Monolith.Core.Extensions
{
    public static class QueryableExtensions
    {
        public static IQueryable<T> Paginate<T>(this IQueryable<T> list, int page, int size = 50)
        {
            var skip = page > 1 ? page * size - size : 0;

            return skip == 0
                ? list?.Take(size)
                : list?.Skip(skip).Take(size);
        }
    }
}
