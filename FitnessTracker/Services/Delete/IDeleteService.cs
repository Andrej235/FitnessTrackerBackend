using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public interface IDeleteService<T> where T : class
    {
        /// <summary>
        /// Deletes the first entity from the database which fits the provided criteria
        /// </summary>
        /// <exception cref="NullReferenceException"></exception>
        Task Delete(Expression<Func<T, bool>> criteria);
    }
}
