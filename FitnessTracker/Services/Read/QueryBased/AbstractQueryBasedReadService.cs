
using FitnessTracker.Data;
using FitnessTracker.Utilities;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace FitnessTracker.Services.Read.QueryBased
{
    public abstract class AbstractQueryBasedReadService<T>(DataContext context) : IReadQueryService<T> where T : class
    {
        protected readonly DataContext context = context;

        public virtual Task<IEnumerable<T>> Get(string? query, int? offset = 0, int? limit = -1, string? include = "all") => Task.Run(() =>
                                                                                                                                      {
                                                                                                                                          IQueryable<T> entitiesQueryable = GetIncluded(include);
                                                                                                                                          if (query is null)
                                                                                                                                              return entitiesQueryable.ApplyOffsetAndLimit(offset, limit);

                                                                                                                                          List<List<string>> keyValuePairsInSearchQuery = SplitQueryString(query);
                                                                                                                                          List<string>? strictKeyValuePair = keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "strict");
                                                                                                                                          bool isStrictModeEnabled = false;

                                                                                                                                          if (strictKeyValuePair != null)
                                                                                                                                          {
                                                                                                                                              isStrictModeEnabled = strictKeyValuePair[1] == "true";
                                                                                                                                              _ = keyValuePairsInSearchQuery.Remove(strictKeyValuePair);
                                                                                                                                          }

                                                                                                                                          entitiesQueryable = isStrictModeEnabled
                                                                                                                                              ? ApplyStrictCriterias(entitiesQueryable, keyValuePairsInSearchQuery)
                                                                                                                                              : ApplyNonStrictCriterias(entitiesQueryable, keyValuePairsInSearchQuery);

                                                                                                                                          return entitiesQueryable.ApplyOffsetAndLimit(offset, limit);
                                                                                                                                      });

        #region Query
        protected abstract Expression<Func<T, bool>> TranslateKeyValueToExpression(string key, string value);

        protected static List<List<string>> SplitQueryString(string query) => query.Split(';')
    .Select(sq => sq.Split('=').Select(x => x.Trim().ToLower()).ToList())
    .Where(x => x.Count == 2)
    .ToList();

        private IEnumerable<Expression<Func<T, bool>>> DecipherQuery(List<List<string>> keyValuePairsInSearchQuery)
        {
            foreach (List<string> keyValue in keyValuePairsInSearchQuery)
            {
                Expression<Func<T, bool>>? current = null;
                try
                {
                    current = TranslateKeyValueToExpression(keyValue[0], keyValue[1]);
                }
                catch (Exception ex)
                {
                    ex.LogError();
                }

                if (current != null)
                    yield return current;
            }
        }

        protected IQueryable<T> ApplyStrictCriterias(IQueryable<T> entitiesQueryable, List<List<string>> keyValuePairsInSearchQuery)
        {
            foreach (Expression<Func<T, bool>> criteria in DecipherQuery(keyValuePairsInSearchQuery))
                entitiesQueryable = entitiesQueryable.Where(criteria);

            return entitiesQueryable;
        }

        protected IQueryable<T> ApplyNonStrictCriterias(IQueryable<T> entitiesQueryable, List<List<string>> keyValuePairsInSearchQuery)
        {
            IEnumerable<Expression<Func<T, bool>>> criterias = DecipherQuery(keyValuePairsInSearchQuery);
            criterias = criterias.Where(x => x.Body is not ConstantExpression);

            return !criterias.Any()
                ? entitiesQueryable
                : criterias
                .SelectMany(x => entitiesQueryable.Where(x))
                .GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .Select(x => x.First())
                .AsQueryable();
        }
        #endregion

        #region Include
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
                || (x.PropertyType.IsGenericType && x.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>)));

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
        #endregion

    }
}
