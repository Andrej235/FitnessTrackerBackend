using System.Linq.Expressions;

namespace FitnessTracker.Services.Read.ExpressionBased
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
        /// <param name="include">
        /// Include string should contain a list of propery names which will be included in the return
        /// <br/>Each item should be separated with a ; (semicolon)
        /// <br/>If one of the items is 'all' every property will be included
        /// <br/>If one of the items is 'none' no property will be included
        /// <br/>Cap insensitive
        /// </param>
        /// <returns>Return a list of entities that fit the provided criteria, if no such entity exists an empty list will be returned</returns>
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> criteria, int? offset = 0, int? limit = -1, string? include = "all");
    }
}
