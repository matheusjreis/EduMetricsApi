using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Reflection;

namespace EduMetricsApi.Infra.Data.Extensions;

public static class EntityFrameworkExtension
{
    private static int _depth { get; set; } = 0;

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