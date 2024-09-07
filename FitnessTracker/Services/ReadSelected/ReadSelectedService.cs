using FitnessTracker.Data;
using FitnessTracker.Services.Read;
using FitnessTracker.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace FitnessTracker.Services.ReadSelected
{
    public class ReadSelectedService<TEntity>(DataContext context) : IReadSingleSelectedService<TEntity>, IReadRangeSelectedService<TEntity> where TEntity : class
    {
        private readonly DataContext context = context;

        public Task<IEnumerable<object>> Get(Expression<Func<TEntity, object>> select, Expression<Func<TEntity, bool>>? criteria, int? offset = 0, int? limit = -1, Func<IWrappedQueryable<TEntity>, IWrappedResult<TEntity>>? queryBuilder = null) => Task.Run(() =>
        {
            if (queryBuilder is null)
                return criteria is null
                    ? context.Set<TEntity>().Select(select).ApplyOffsetAndLimit(offset, limit)
                    : context.Set<TEntity>().Where(criteria).Select(select).ApplyOffsetAndLimit(offset, limit);

            IWrappedResult<TEntity> query = queryBuilder.Invoke(context.Set<TEntity>().Wrap());
            IQueryable<TEntity>? source = Unwrap(query);

            if (source is null)
                return [];

            return criteria is null
                ? source.Select(select).ApplyOffsetAndLimit(offset, limit)
                : source.Where(criteria).Select(select).ApplyOffsetAndLimit(offset, limit);
        });

        public async Task<T?> Get<T>(Expression<Func<TEntity, T>> select, Expression<Func<TEntity, bool>> criteria, Func<IWrappedQueryable<TEntity>, IWrappedResult<TEntity>>? queryBuilder = null)
        {
            if (queryBuilder is null)
                return await context.Set<TEntity>().Where(criteria).Select(select).FirstOrDefaultAsync();

            IWrappedResult<TEntity> query = queryBuilder.Invoke(context.Set<TEntity>().Wrap());
            IQueryable<TEntity>? source = Unwrap(query);

            if (source is null)
                return default;

            return await source.Where(criteria).Select(select).FirstOrDefaultAsync();
        }

        private static IQueryable<TEntity>? Unwrap(IWrappedResult<TEntity> source) => (source as WrappedQueryable<TEntity>)?.Source ?? (source as WrappedOrderedQueryable<TEntity>)?.Source ?? null;
    }
}

