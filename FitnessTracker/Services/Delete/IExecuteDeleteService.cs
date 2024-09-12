using System.Linq.Expressions;

namespace FitnessTracker.Services.Delete
{
    public interface IExecuteDeleteService<T> where T : class
    {
        Task Delete(Expression<Func<T, bool>> deleteCriteria);
    }
}
