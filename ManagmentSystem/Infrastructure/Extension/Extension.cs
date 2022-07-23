using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Reflection;

namespace Infrastructure.Extension;

public static class Extension
{
    public static IQueryable<T> GetPage<T>(this IQueryable<T> query, int PageNo = 1, int PageSize = 20, string OrderBy = "CreateDate", bool IsDesc = true)
    {
        if (char.IsLower(OrderBy[0]))
        {
            OrderBy = string.Concat(OrderBy[0].ToString().ToUpper(), OrderBy[1..]);
        }

        if (IsDesc)
        {
            return query.OrderByDescending(x => EF.Property<object>(x, OrderBy)).Skip((PageNo - 1) * PageSize).Take(PageSize);
        }
        else
        {
            return query.OrderBy(x => EF.Property<object>(x, OrderBy)).Skip((PageNo - 1) * PageSize).Take(PageSize);
        }
    }

    public static TSelf TrimStringProperties<TSelf>(this TSelf input)
    {
        IEnumerable<PropertyInfo>? stringProperties = input?.GetType().GetProperties()
                                                            .Where(p => p.PropertyType == typeof(string)
                                                                     && p.CanWrite);

        if (stringProperties != null && stringProperties.Any())
        {
            foreach (var stringProperty in stringProperties)
            {
                string? currentValue = (string?)stringProperty.GetValue(input, null);

                if (currentValue != null) stringProperty.SetValue(input, currentValue.Trim(), null);
            }
        }

        return input;
    }
}