using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public interface IReadRangeService<T> where T : class
    {
        /// <summary>
        /// Finds all entities in the database which fit the provided criteria
        /// </summary>
        /// <param name="criteria">Criteria which is used to search for specific entities in the database</param>
        /// <param name="offset">
        /// Number of entities which will be skipped when creating the output list
        /// <br/>If the value is 0, no entities will be skipped
        /// <br/>If the value is null, no entities will be skipped
        /// </param>
        /// <param name="limit">
        /// Maximum number of entities which will be included in the output list
        /// <br/>If a negative value is entered, output will include all entities which fit the criteria
        /// <br/>If the value is null, output will include all entities which fit the criteria
        /// </param>
        /// <param name="queryBuilder">
        /// Used to further modify the query
        /// It allows 4 methods: Include, ThenInclude, OrderBy and OrderByDescending
        /// </param>
        /// <returns>Return a list of entities that fit the provided criteria, if no such entity exists an empty list will be returned</returns>
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? criteria, int? offset = 0, int? limit = -1, Func<IWrappedQueryable<T>, IWrappedResult<T>>? queryBuilder = null);
    }
}
