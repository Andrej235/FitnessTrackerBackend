using FitnessTracker.Data;
using FitnessTracker.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read
{
    public class ReadService<T>(DataContext context) : IReadSingleService<T>, IReadRangeService<T> where T : class
    {
        private readonly DataContext context = context;

        public Task<T?> Get(Expression<Func<T, bool>> criteria, Func<IWrappedQueryable<T>, IWrappedResult<T>>? include = null) => include is null
                ? context.Set<T>().FirstOrDefaultAsync(criteria)
                : Unwrap(include.Invoke(context.Set<T>().Wrap()))?.FirstOrDefaultAsync(criteria) ?? Task.FromResult<T?>(null);

        public Task<IEnumerable<T>> Get(Expression<Func<T, bool>>? criteria, int? offset = 0, int? limit = -1, Func<IWrappedQueryable<T>, IWrappedResult<T>>? include = null) => Task.Run(() =>
        {
            if (include is null)
                return criteria is null
                    ? context.Set<T>().ApplyOffsetAndLimit(offset, limit)
                    : context.Set<T>().Where(criteria).ApplyOffsetAndLimit(offset, limit);

            IWrappedResult<T> includeResult = include.Invoke(context.Set<T>().Wrap());
            IQueryable<T>? source = Unwrap(includeResult);

            if (source is null)
                return [];

            return criteria is null
                ? source.ApplyOffsetAndLimit(offset, limit)
                : source.Where(criteria).ApplyOffsetAndLimit(offset, limit);
        });


        private static IQueryable<T>? Unwrap(IWrappedResult<T> source) => (source as WrappedQueryable<T>)?.Source ?? (source as WrappedOrderedQueryable<T>)?.Source ?? null;
    }
}
