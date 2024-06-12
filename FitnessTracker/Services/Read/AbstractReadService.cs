using Microsoft.EntityFrameworkCore;
using ProjectGym.Data;
using ProjectGym.Utilities;
using System.Linq.Expressions;
using System.Reflection;

namespace ProjectGym.Services.Read
{
    public abstract class AbstractReadService<T>(ExerciseContext context) : IReadService<T> where T : class
    {
        public virtual async Task<T?> Get(Expression<Func<T, bool>> criteria, string? include = "all")
        {
            IQueryable<T> entitesQueryable = GetIncluded(include);
            T? entity = await entitesQueryable.FirstOrDefaultAsync(criteria);
            return entity;
        }

        public Task<T?> Get(object id, string? include = "all")
        {
            return Task.Run(() =>
            {
                IQueryable<T> entitesQueryable = GetIncluded(include);
                bool CheckPrimaryKey(T e)
                {
                    var idProp = e.GetType().GetProperty("Id");
                    if (idProp == null)
                        return false;

                    return Convert.ToString(idProp.GetValue(e)) == Convert.ToString(id);
                }

                T? entity = entitesQueryable.AsEnumerable().FirstOrDefault(CheckPrimaryKey);
                return entity;
            });
        }

        public virtual Task<List<T>> Get(Expression<Func<T, bool>> criteria, int? offset = 0, int? limit = -1, string? include = "all")
        {
            return Task.Run(() =>
            {
                IQueryable<T> entitesQueryable = GetIncluded(include);
                return ApplyOffsetAndLimit(entitesQueryable.Where(criteria), offset, limit);
            });
        }

        public virtual Task<List<T>> Get(string? query, int? offset = 0, int? limit = -1, string? include = "all")
        {
            return Task.Run(() =>
            {
                var entitiesQueryable = GetIncluded(include);
                if (query is null)
                    return ApplyOffsetAndLimit(entitiesQueryable, offset, limit);

                var keyValuePairsInSearchQuery = SplitQueryString(query);
                List<string>? strictKeyValuePair = keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "strict");
                bool isStrictModeEnabled = false;

                if (strictKeyValuePair != null)
                {
                    isStrictModeEnabled = strictKeyValuePair[1] == "true";
                    keyValuePairsInSearchQuery.Remove(strictKeyValuePair);
                }

                if (isStrictModeEnabled)
                {
                    foreach (var criteria in DecipherQuery(keyValuePairsInSearchQuery))
                        entitiesQueryable = entitiesQueryable.Where(criteria);
                }
                else
                {
                    entitiesQueryable = ApplyNonStrictCriterias(entitiesQueryable, DecipherQuery(keyValuePairsInSearchQuery));
                }

                return ApplyOffsetAndLimit(entitiesQueryable, offset, limit);
            });
        }

        protected abstract Expression<Func<T, bool>> TranslateKeyValueToExpression(string key, string value);

        protected static IEnumerable<string>? SplitIncludeString(string? include) => include?.ToLower().Replace(" ", "").Split(',').Where(x => !string.IsNullOrWhiteSpace(x));

        protected static List<List<string>> SplitQueryString(string query) => query.Split(';')
            .Select(sq => sq.Split('=').Select(x => x.Trim().ToLower()).ToList())
            .Where(x => x.Count == 2)
            .ToList();

        protected IEnumerable<Expression<Func<T, bool>>> DecipherQuery(string query)
        {
            var keyValuePairsInSearchQuery = SplitQueryString(query);
            foreach (var keyValue in keyValuePairsInSearchQuery)
            {
                Expression<Func<T, bool>>? current = null;
                try
                {
                    current = TranslateKeyValueToExpression(keyValue[0], keyValue[1]);
                }
                catch (Exception ex)
                {
                    LogDebugger.LogError(ex);
                }

                if (current == null)
                    continue;
                yield return current;
            }
        }
        protected IEnumerable<Expression<Func<T, bool>>> DecipherQuery(List<List<string>> keyValuePairsInSearchQuery)
        {
            foreach (var keyValue in keyValuePairsInSearchQuery)
            {
                Expression<Func<T, bool>>? current = null;
                try
                {
                    current = TranslateKeyValueToExpression(keyValue[0], keyValue[1]);
                }
                catch (Exception ex)
                {
                    LogDebugger.LogError(ex);
                }

                if (current == null)
                    continue;
                yield return current;
            }
        }

        protected virtual IQueryable<T> ApplyNonStrictCriterias(IQueryable<T> entitiesQueryable, IEnumerable<Expression<Func<T, bool>>> criterias)
        {
            criterias = criterias.Where(x => x.Body is not ConstantExpression);

            if (!criterias.Any())
                return entitiesQueryable;

            return criterias
                .SelectMany(x => entitiesQueryable.Where(x))
                .GroupBy(x => x)
                .OrderByDescending(g => g.Count())
                .Select(x => x.First())
                .AsQueryable();
        }

        protected virtual List<T> ApplyOffsetAndLimit(IQueryable<T> queryable, int? offset = 0, int? limit = -1)
        {
            queryable = queryable.Skip(offset ?? 0);

            if (limit != null && limit >= 0)
                queryable = queryable.Take(limit ?? 0);

            return [.. queryable];
        }

        protected virtual IQueryable<T> GetIncluded(string? includeString)
        {
            IEnumerable<string>? include = SplitIncludeString(includeString);
            IQueryable<T> entitiesIncluding = context.Set<T>().AsQueryable();
            if (include is null || !include.Any() || include.Contains("none"))
                return entitiesIncluding;

            ParameterExpression parameter = Expression.Parameter(typeof(T), "x");
            var navigationProperties = typeof(T).GetProperties()
                .Where(p => p.PropertyType.IsGenericType && p.PropertyType.GetGenericTypeDefinition() == typeof(IEnumerable<>));

            if (include.Contains("all"))
            {
                foreach (var navigationProperty in navigationProperties)
                    entitiesIncluding = Include(entitiesIncluding, navigationProperty.Name);

                return entitiesIncluding;
            }

            IEnumerable<PropertyInfo> includedNavigationProperties = navigationProperties.Where(navigationProperty => include.Any(includeMember => navigationProperty.Name.Contains(includeMember, StringComparison.CurrentCultureIgnoreCase)));
            foreach (var navigationProperty in includedNavigationProperties)
                entitiesIncluding = Include(entitiesIncluding, navigationProperty.Name);

            return entitiesIncluding;
        }

        private static IQueryable<TEntity> Include<TEntity>(IQueryable<TEntity> query, string propertyName) where TEntity : class
        {
            var parameter = Expression.Parameter(typeof(TEntity), "x");
            var property = Expression.Property(parameter, propertyName);
            var lambda = Expression.Lambda<Func<TEntity, object>>(property, parameter);

            return query.Include(lambda);
        }

        //**************************************************************
        //Less efficient than just doing it in function
        //protected static bool IsStrictModeEnabledInQuery(string query) => SplitQueryString(query).FirstOrDefault(kvp => kvp[0] == "strict")?[1] == "true";
        //protected static bool IsStrictModeEnabledInQuery(List<List<string>> keyValuePairsInSearchQuery) => keyValuePairsInSearchQuery.FirstOrDefault(kvp => kvp[0] == "strict")?[1] == "true";
        //**************************************************************
    }
}
