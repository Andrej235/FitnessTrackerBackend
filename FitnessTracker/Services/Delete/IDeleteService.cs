using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public interface IDeleteService<T> where T : class
    {
        /// <summary>
        /// Deletes all entities that match the delete criteria
        /// </summary>
        /// <param name="deleteCriteria">Criteria to match</param>
        ///<exception cref="Exceptions.NotFoundException"/>
        Task Delete(Expression<Func<T, bool>> deleteCriteria);
    }
}
