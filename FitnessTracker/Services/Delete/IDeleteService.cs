using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public interface IDeleteService<T> where T : class
    {
        Task Delete(Expression<Func<T, bool>> deleteCriteria);
    }
}
