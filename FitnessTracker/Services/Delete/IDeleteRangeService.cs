using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public interface IDeleteRangeService<T>
    {
        /// <summary>
        /// Deletes all entities from the database which fit the provided criteria
        /// </summary>
        /// <returns>
        /// true - if any entities were deleted
        /// <br/>false - if no entities were deleted
        /// </returns>
        Task<bool> Delete(Expression<Func<T, bool>> criteria);
    }
}
