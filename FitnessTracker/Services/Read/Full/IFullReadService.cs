using System.Linq.Expressions;

namespace FitnessTracker.Services.Read.Full
{
    public interface IFullReadService<T> where T : class
    {
        Task<T?> Get(Expression<Func<T, bool>> criteria, Func<IWrappedQueryable<T>, IWrappedIncludableQueryable<T, object>>? include = null);
    }
}
