using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace EduMetricsApi.Infra.Data.Extensions;

public static class EntityFrameworkExtension
{
    private static int _depth { get; set; } = 0;

    public static IQueryable<TEntity> IncludeRecursively<TEntity>(this IQueryable<TEntity> queryable, ICollection<string>? exclude = null) where TEntity : class
    {
        var entityType = typeof(TEntity);
        var properties = entityType.GetProperties().Where(w => exclude == null ? w == w : !exclude.Contains(w.Name));

        foreach (var property in properties)
        {
            var propertyType = property.PropertyType;

            if (propertyType.IsClass && propertyType != typeof(string) && (exclude is null || (exclude is not null && !exclude.Contains(property.Name))))
            {
                var p = new List<string>();
                queryable = queryable.Include(property.Name);

                _depth = 0;
                GetPropertiesRecursive(Type.GetType($"{property.PropertyType.FullName}, {property.PropertyType.Assembly.FullName}")!, p);

                foreach (var item in p)
                {
                    queryable.Include(property.Name + "." + item);
                }
            }

            // SE FOR UMA LISTA
            if (propertyType.IsAbstract)
            {
                var listProp = propertyType.GetGenericArguments()[0];
                foreach (var props in listProp.GetProperties())
                {
                    if (props.PropertyType.IsClass && props.PropertyType != typeof(string))
                    {
                        if (exclude is not null && exclude.Contains(property.Name))
                        {
                            continue;
                        }

                        queryable = queryable.Include($"{listProp.Name}.{props.Name}");
                    }
                }

                queryable = queryable.Include(listProp.Name);
            }
        }

        return queryable;
    }

    private static void GetPropertiesRecursive(Type type, List<string> propertyNames, string prefix = "", int depth = 10)
    {
        var properties = type.GetProperties();

        foreach (PropertyInfo property in properties)
        {
            if (property.Name == "Item" || propertyNames.Any(x => x.Equals(property.Name)))
            {
                continue;
            }

            if (property.PropertyType.IsClass && property.PropertyType != typeof(string))
            {
                propertyNames.Add(prefix + property.Name);

                if (propertyNames.Count > 1)
                {
                    _depth++;
                }

                if (_depth <= depth && property.PropertyType.IsClass && property.PropertyType != typeof(string))
                {
                    GetPropertiesRecursive(property.PropertyType, propertyNames, propertyNames[_depth] + ".");
                }
            }
        }
    }

    public static IQueryable<TEntity> WhereGen<TEntity>(this IQueryable<TEntity> queryable, string property, object value) where TEntity : class
    {
        var lambda = GetDynamicWhereClause<TEntity>(property, value);
        return queryable.Where(lambda);
    }

    private static Expression<Func<T, bool>> GetDynamicWhereClause<T>(string propertyName, object value)
    {
        var entityType = typeof(T);
        var parameter = Expression.Parameter(entityType, "x");
        var left = Expression.Property(parameter, propertyName);
        var right = Expression.Constant(value);
        var body = Expression.Equal(left, right);
        return Expression.Lambda<Func<T, bool>>(body, parameter);
    }
}