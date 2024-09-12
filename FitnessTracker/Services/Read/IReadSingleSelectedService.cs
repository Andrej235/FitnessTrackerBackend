using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public interface IReadSingleSelectedService<TEntity> where TEntity : class
    {
        Task<T?> Get<T>(Expression<Func<TEntity, T>> select, Expression<Func<TEntity, bool>> criteria, Func<IWrappedQueryable<TEntity>, IWrappedResult<TEntity>>? queryBuilder = null);
    }
}
