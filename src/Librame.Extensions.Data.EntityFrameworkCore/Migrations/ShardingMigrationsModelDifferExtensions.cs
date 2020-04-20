#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Librame.Extensions.Data.Migrations
{
    static class ShardingMigrationsModelDifferExtensions
    {
        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
        public static TValue GetOrAddNew<TKey, TValue>(
            this IDictionary<TKey, TValue> source,
            TKey key)
            where TValue : new()
        {
            if (!source.TryGetValue(key, out var value))
            {
                value = new TValue();
                source.Add(key, value);
            }

            return value;
        }

        public static IEnumerable<Type> GetBaseTypes(this Type type)
        {
            type = type.GetTypeInfo().BaseType;

            while (type != null)
            {
                yield return type;

                type = type.GetTypeInfo().BaseType;
            }
        }
        public static bool IsSameAs(this MemberInfo propertyInfo, MemberInfo otherPropertyInfo)
            => propertyInfo == null
                ? otherPropertyInfo == null
                : (otherPropertyInfo == null
                    ? false
                    : Equals(propertyInfo, otherPropertyInfo)
                      || (propertyInfo.Name == otherPropertyInfo.Name
                          && (propertyInfo.DeclaringType == otherPropertyInfo.DeclaringType
                              || propertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(otherPropertyInfo.DeclaringType)
                              || otherPropertyInfo.DeclaringType.GetTypeInfo().IsSubclassOf(propertyInfo.DeclaringType)
                              || propertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains(otherPropertyInfo.DeclaringType)
                              || otherPropertyInfo.DeclaringType.GetTypeInfo().ImplementedInterfaces.Contains(propertyInfo.DeclaringType))));

    }
}
