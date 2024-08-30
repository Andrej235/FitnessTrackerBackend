using FitnessTracker.Data;
using FitnessTracker.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read.Full
{
    public class FullReadService<T>(DataContext context) : IFullReadService<T>, IFullReadRangeService<T> where T : class
    {
        private readonly DataContext context = context;

        public Task<T?> Get(Expression<Func<T, bool>> criteria, Func<IWrappedQueryable<T>, IWrappedResult<T>>? include = null) => include is null
                ? context.Set<T>().FirstOrDefaultAsync(criteria)
                : Unwrap(include.Invoke(context.Set<T>().Wrap()))?.FirstOrDefaultAsync(criteria) ?? Task.FromResult<T?>(null);

        public Task<IEnumerable<T>> Get(Expression<Func<T, bool>> criteria, int? offset = 0, int? limit = -1, Func<IWrappedQueryable<T>, IWrappedResult<T>>? include = null) => Task.Run<IEnumerable<T>>(() =>
        {
            if (include is null)
                return context.Set<T>().Where(criteria).ApplyOffsetAndLimit(offset, limit);

            IWrappedResult<T> includeResult = include.Invoke(context.Set<T>().Wrap());

            if (includeResult is WrappedQueryable<T> wrappedQueryable)
                return wrappedQueryable.Source.OrderBy(x => 1).Where(criteria).ApplyOffsetAndLimit(offset, limit);

            if (includeResult is WrappedOrderedQueryable<T> wrappedOrderedQueryable)
                return wrappedOrderedQueryable.Source.Where(criteria).ApplyOffsetAndLimit(offset, limit);

            return [];
        });


        private static IQueryable<T>? Unwrap(IWrappedResult<T> source) => (source as WrappedQueryable<T>)?.Source ?? (source as WrappedOrderedQueryable<T>)?.Source ?? null;
    }
}
