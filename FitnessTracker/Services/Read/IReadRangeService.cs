using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public interface IReadRangeService<T> where T : class
    {
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? criteria, int? offset = 0, int? limit = -1, Func<IWrappedQueryable<T>, IWrappedResult<T>>? queryBuilder = null);
    }
}
