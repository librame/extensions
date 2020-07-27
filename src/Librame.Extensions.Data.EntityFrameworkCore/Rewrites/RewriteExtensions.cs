#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace Microsoft.EntityFrameworkCore.Migrations.Internal
{
    internal static class RewriteExtensions
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
            source.NotNull(nameof(source));

            if (!source.TryGetValue(key, out var value))
            {
                value = new TValue();
                source.Add(key, value);
            }

            return value;
        }


        /// <summary>
        ///     This API supports the Entity Framework Core infrastructure and is not intended to be used
        ///     directly from your code. This API may change or be removed in future releases.
        /// </summary>
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
