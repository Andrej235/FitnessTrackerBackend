using FitnessTracker.Data;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using System.Linq.Expressions;

namespace FitnessTracker.Services.ReadSelected
{
    public class ReadSelectedService<T>(DataContext context) : IReadRangeSelectedService<T> where T : class
    {
        private readonly DataContext context = context;

        public Task<IEnumerable<object>> Get(Expression<Func<T, object>> select, Expression<Func<T, bool>>? criteria, int? offset = 0, int? limit = -1, Func<IWrappedQueryable<T>, IWrappedResult<T>>? queryBuilder = null) => Task.Run(() =>
        {
            if (queryBuilder is null)
                return criteria is null
                    ? context.Set<T>().Select(select).ApplyOffsetAndLimit(offset, limit)
                    : context.Set<T>().Where(criteria).Select(select).ApplyOffsetAndLimit(offset, limit);

            IWrappedResult<T> includeResult = queryBuilder.Invoke(context.Set<T>().Wrap());
            IQueryable<T>? source = Unwrap(includeResult);

            if (source is null)
                return [];

            return criteria is null
                ? source.Select(select).ApplyOffsetAndLimit(offset, limit)
                : source.Where(criteria).Select(select).ApplyOffsetAndLimit(offset, limit);
        });

        private static IQueryable<T>? Unwrap(IWrappedResult<T> source) => (source as WrappedQueryable<T>)?.Source ?? (source as WrappedOrderedQueryable<T>)?.Source ?? null;
    }
}
