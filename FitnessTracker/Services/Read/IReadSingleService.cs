using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public interface IReadSingleService<T> where T : class
    {
        /// <summary>
        /// Finds the first entity in database which fits the <paramref name="criteria"/>
        /// </summary>
        /// <param name="criteria">Expression used to find the entity</param>
        /// <param name="queryBuilder">
        /// Used to further modify the query
        /// Allows 5 methods: Include, ThenInclude, OrderBy, OrderByDescending and AsNoTracking
        /// </param>
        /// <returns>First entity that fits the <paramref name="criteria"/> or if such entity doesn't exist, null</returns>
        Task<T?> Get(Expression<Func<T, bool>> criteria, Func<IWrappedQueryable<T>, IWrappedResult<T>>? queryBuilder = null);
    }
}
