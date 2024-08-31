using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public interface IReadSingleService<T> where T : class
    {
        Task<T?> Get(Expression<Func<T, bool>> criteria, Func<IWrappedQueryable<T>, IWrappedResult<T>>? queryBuilder = null);
    }
}
