using System.Linq.Expressions;

namespace FitnessTracker.Services.Read.Count
{
    public interface ICountService<T> where T : class
    {
        Task<int> Count(Expression<Func<T, bool>> criteria);
    }
}
