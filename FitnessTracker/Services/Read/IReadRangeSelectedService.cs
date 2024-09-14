using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public interface IReadRangeSelectedService<TEntity> where TEntity : class
    {
        Task<IEnumerable<T>> Get<T>(Expression<Func<TEntity, T>> select, Expression<Func<TEntity, bool>>? criteria, int? offset = 0, int? limit = -1, Func<IWrappedQueryable<TEntity>, IWrappedResult<TEntity>>? queryBuilder = null);
    }
}
