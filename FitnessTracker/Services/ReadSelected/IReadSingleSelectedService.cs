using FitnessTracker.Services.Read;
using System.Linq.Expressions;

namespace FitnessTracker.Services.ReadSelected
{
    public interface IReadSingleSelectedService<T> where T : class
    {
        Task<object?> Get(Expression<Func<T, object>> select, Expression<Func<T, bool>> criteria, Func<IWrappedQueryable<T>, IWrappedResult<T>>? queryBuilder = null);
    }
}
