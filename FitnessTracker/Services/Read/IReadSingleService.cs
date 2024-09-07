using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public interface IReadSingleService<T> where T : class
    {
        /// <summary>
        /// Finds a first entity in database which fits the provided criteria
        /// </summary>
        /// <param name="criteria">Criteria which is used to search for a specific entity in the database</param>
        /// <param name="queryBuilder">
        /// Used to further modify the query
        /// It allows 5 methods: Include, ThenInclude, OrderBy, OrderByDescending and AsNoTracking
        /// </param>
        /// <returns>Returns a first entity that fits the provided criteria or if such entity doesn't exist, null</returns>
        Task<T?> Get(Expression<Func<T, bool>> criteria, Func<IWrappedQueryable<T>, IWrappedResult<T>>? queryBuilder = null);
    }
}
