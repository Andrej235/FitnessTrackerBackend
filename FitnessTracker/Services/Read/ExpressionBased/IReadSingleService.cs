using System.Linq.Expressions;

namespace FitnessTracker.Services.Read.ExpressionBased
{
    public interface IReadSingleService<T> where T : class
    {
        /// <summary>
        /// Finds a first entity in database which fits the provided criteria
        /// </summary>
        /// <param name="criteria">Criteria which is used to search for a specific entity in the database</param>
        /// <param name="include">
        /// Include string should contain a list of propery names which will be included in the return
        /// <br/>Each item should be separated with a ; (semicolon)
        /// <br/>If one of the items is 'all' every property will be included
        /// <br/>If one of the items is 'none' no property will be included
        /// <br/>Cap insensitive
        /// </param>
        /// <returns>Returns a first entity that fits the provided criteria or if such entity doesn't exist, null</returns>
        Task<T?> Get(Expression<Func<T, bool>> criteria, string? include = "all");
    }
}
