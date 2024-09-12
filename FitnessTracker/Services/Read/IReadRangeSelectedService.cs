using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public interface IReadRangeSelectedService<T> where T : class
    {
        Task<IEnumerable<object>> Get(Expression<Func<T, object>> select, Expression<Func<T, bool>>? criteria, int? offset = 0, int? limit = -1, Func<IWrappedQueryable<T>, IWrappedResult<T>>? queryBuilder = null);
    }
}
