﻿using FitnessTracker.Data;
using FitnessTracker.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace FitnessTracker.Services.Read.ExpressionBased
{
    public class ReadExpressionService<T>(DataContext context) : IReadSingleService<T>, IReadRangeService<T> where T : class
    {
        protected readonly DataContext context = context;

        public virtual async Task<T?> Get(Expression<Func<T, bool>> criteria, string? include = "all")
        {
            IQueryable<T> entitesQueryable = GetIncluded(include);
            T? entity = await entitesQueryable.FirstOrDefaultAsync(criteria);
            return entity;
        }

        public virtual Task<IEnumerable<T>> Get(Expression<Func<T, bool>> criteria, int? offset = 0, int? limit = -1, string? include = "all") => Task.Run(() =>
                                                                                                                                                           {
                                                                                                                                                               IQueryable<T> entitesQueryable = GetIncluded(include);
                                                                                                                                                               return entitesQueryable.Where(criteria).ApplyOffsetAndLimit(offset, limit);
                                                                                                                                                           });

        protected static IEnumerable<string>? SplitIncludeString(string? include) => include?.ToLower().Replace(" ", "").Split(',').Where(x => !string.IsNullOrWhiteSpace(x));

        protected virtual IQueryable<T> GetIncluded(string? includeString)
        {
            IEnumerable<string>? include = SplitIncludeString(includeString);
            IQueryable<T> entitiesIncluding = context.Set<T>().AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluding;

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            IEnumerable<PropertyInfo> navigationProperties = typeof(T).GetProperties().Where(x =>
                (x.PropertyType.IsClass && x.PropertyType != typeof(string))
                || (x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(ICollection<>)));

            if (include.Contains("all"))
            {
                foreach (PropertyInfo? navigationProperty in navigationProperties)
                    entitiesIncluding = Include(entitiesIncluding, navigationProperty.Name);

                return entitiesIncluding;
            }

            IEnumerable<PropertyInfo> includedNavigationProperties = navigationProperties.Where(navigationProperty => include.Any(includeMember => navigationProperty.Name.Contains(includeMember, StringComparison.CurrentCultureIgnoreCase)));
            foreach (PropertyInfo navigationProperty in includedNavigationProperties)
                entitiesIncluding = Include(entitiesIncluding, navigationProperty.Name);

            return entitiesIncluding;
        }

        protected static IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> query, string propertyName) where TEntity : class
        {
            ParameterExpression parameter = Expression.Parameter(typeof(TEntity), "x");
            MemberExpression property = Expression.Property(parameter, propertyName);
            Expression<Func<TEntity, object>> lambda = Expression.Lambda<Func<TEntity, object>>(property, parameter);

            return query.Include(lambda);
        }
    }
}
