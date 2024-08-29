﻿using FitnessTracker.Services.Read.Full;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;

namespace FitnessTracker.Services.Read.Full
{
    public interface IWrappedQueryable<out TEntity>;
    public interface IWrappedIncludableQueryable<out TEntity, out TProperty> : IWrappedQueryable<TEntity>;
    public interface ITest<TEntity, TProperty>
    {
        IIncludableQueryable<TEntity, TProperty> IncludableSource { get; }
    }

    public record WrappedQueryable<TEntity>(IQueryable<TEntity> Source) : IWrappedQueryable<TEntity>;
    public record WrappedIncludableQueryable<TEntity, TProperty>(IIncludableQueryable<TEntity, TProperty> IncludableSource) : WrappedQueryable<TEntity>(IncludableSource), ITest<TEntity, TProperty>, IWrappedIncludableQueryable<TEntity, TProperty>;

    public static class WrappedIncludableQueryableExtensions
    {
        public static IWrappedQueryable<TEntity> Wrap<TEntity>(this IQueryable<TEntity> source) where TEntity : class => new WrappedQueryable<TEntity>(source);

        public static IWrappedQueryable<TEntity> Include<TEntity>(
            this IWrappedQueryable<TEntity> source,
            [NotParameterized] string navigationPropertyPath)
            where TEntity : class => new WrappedQueryable<TEntity>((source as WrappedQueryable<TEntity> ?? throw new InvalidOperationException()).Source.Include(navigationPropertyPath));

        public static IWrappedIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(
            this IWrappedQueryable<TEntity> source,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class => new WrappedIncludableQueryable<TEntity, TProperty>(((source as WrappedQueryable<TEntity>) ?? throw new InvalidOperationException()).Source.Include(navigationPropertyPath));

        public static IWrappedIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty, TPreviousProperty>(
            this IWrappedIncludableQueryable<TEntity, TPreviousProperty> source,
            Expression<Func<TEntity, TProperty>> navigationPropertyPath)
            where TEntity : class => new WrappedIncludableQueryable<TEntity, TProperty>(((source as WrappedIncludableQueryable<TEntity, TPreviousProperty>) ?? throw new InvalidOperationException()).Source.Include(navigationPropertyPath));

        public static IWrappedIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IWrappedIncludableQueryable<TEntity, TPreviousProperty> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class => new WrappedIncludableQueryable<TEntity, TProperty>((source as WrappedIncludableQueryable<TEntity, TPreviousProperty> ?? throw new InvalidOperationException()).IncludableSource.ThenInclude(navigationPropertyPath));

        public static IWrappedIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(
            this IWrappedIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source,
            Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath)
            where TEntity : class => new WrappedIncludableQueryable<TEntity, TProperty>(
                (
                    (
                        (source as WrappedIncludableQueryable<TEntity, ICollection<TPreviousProperty>>)?.IncludableSource
                        ?? (source as WrappedIncludableQueryable<TEntity, IOrderedEnumerable<TPreviousProperty>>)?.IncludableSource
                        ?? (source as WrappedIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>>)?.IncludableSource
                        ?? (source as WrappedIncludableQueryable<TEntity, IQueryable<TPreviousProperty>>)?.IncludableSource
                    ) ?? throw new InvalidOperationException()
                ).ThenInclude(navigationPropertyPath)
            );
    }
}
