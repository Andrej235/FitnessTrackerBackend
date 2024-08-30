using System.Linq.Expressions;

namespace FitnessTracker.Services.Read.Full
{
    public interface IFullReadRangeService<T> where T : class
    {
        Task<IEnumerable<T>> Get(Expression<Func<T, bool>> criteria, int? offset = 0, int? limit = -1, Func<IWrappedQueryable<T>, IWrappedResult<T>>? include = null);
    }
}
