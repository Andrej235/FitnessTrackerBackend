using FitnessTracker.Data;
using FitnessTracker.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read.Full
{
    public class FullReadService<T>(DataContext context) : IFullReadService<T>, IFullReadRangeService<T> where T : class
    {
        private readonly DataContext context = context;

        public Task<T?> Get(Expression<Func<T, bool>> criteria, Func<IWrappedQueryable<T>, IWrappedIncludableQueryable<T, object>>? include = null) => include is null
                ? context.Set<T>().FirstOrDefaultAsync(criteria)
                : Unwrap(include.Invoke(context.Set<T>().Wrap()))?.FirstOrDefaultAsync(criteria) ?? Task.FromResult<T?>(null);

        public Task<IEnumerable<T>> Get(Expression<Func<T, bool>> criteria, int? offset = 0, int? limit = -1, Func<IWrappedQueryable<T>, IWrappedIncludableQueryable<T, object>>? include = null) => Task.Run(() => include is null
                ? context.Set<T>().Where(criteria).ApplyOffsetAndLimit(offset, limit)
                : Unwrap(include.Invoke(context.Set<T>().Wrap()))?.Where(criteria).ApplyOffsetAndLimit(offset, limit) ?? []);

        private static IQueryable<T>? Unwrap(IWrappedIncludableQueryable<T, object> source) => (source as WrappedQueryable<T>)?.Source ?? null;
    }
}
