using FitnessTracker.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FitnessTracker.Services.IncludeService
{
    public interface IFakeQueryable<out TEntity>
    {
    }

    public class FakeQueryable<TEntity>(IQueryable<TEntity> source) : IFakeQueryable<TEntity>
    {
        public IQueryable<TEntity> Source => source;
    }

    public interface IFakeIncludableQueryable<out TEntity, out TProperty> : IFakeQueryable<TEntity>
    {
    }

    public class FakeIncludableQueryable<TEntity, TProperty>(IIncludableQueryable<TEntity, TProperty> source) : IFakeIncludableQueryable<TEntity, TProperty>
    {
        public IIncludableQueryable<TEntity, TProperty> Source => source;

        public static implicit operator FakeQueryable<TEntity>(FakeIncludableQueryable<TEntity, TProperty> source) => new(source.Source);
    }

    public static class IncludableQueryableWrapper
    {
        public static IFakeQueryable<TEntity> ToFake<TEntity>(this IQueryable<TEntity> source) where TEntity : class => new FakeQueryable<TEntity>(source);

        public static IFakeQueryable<TEntity> Include<TEntity>(
            this IFakeQueryable<TEntity> source,
            [NotParameterized] string navigationPropertyPath)
            where TEntity : class => new FakeQueryable<TEntity>((source as FakeQueryable<TEntity> ?? throw new InvalidOperationException()).Source.Include(navigationPropertyPath));

        public static IFakeIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(
            this IFakeQueryable<TEntity> source,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class => new FakeIncludableQueryable<TEntity, TProperty>((source as FakeQueryable<TEntity> ?? throw new InvalidOperationException()).Source.Include(navigationPropertyPath));

        public static IFakeIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IFakeIncludableQueryable<TEntity, TPreviousProperty> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class => new FakeIncludableQueryable<TEntity, TProperty>((source as FakeIncludableQueryable<TEntity, TPreviousProperty> ?? throw new InvalidOperationException()).Source.ThenInclude(navigationPropertyPath));

        public static IFakeIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IFakeIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class => new FakeIncludableQueryable<TEntity, TProperty>((source as FakeIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> ?? throw new InvalidOperationException()).Source.ThenInclude(navigationPropertyPath));
    }

    public static class Test
    {
        public static void Test123()
        {
            IQueryable<User> queryable = null!;

            _ = queryable.ToFake()
                          .Include(x => x.CurrentSplit!)
                          .ThenInclude(x => x.Creator)
                          .ThenInclude(x => x.CreatedSplits)
                          .ThenInclude(x => x.Likes)
                          .ThenInclude(x => x.CreatedWorkouts)
                          .ThenInclude(x => x.Favorites)
                          .Include(x => x.CompletedWorkouts)
                          .ThenInclude(x => x.Workout!)
                          .ThenInclude(x => x.Likes)
                          .Include(x => x.CurrentSplit)
                          .Include(x => x.CreatedWorkouts);
        }
    }
}
