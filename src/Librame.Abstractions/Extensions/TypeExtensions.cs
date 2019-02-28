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
using System.Reflection;

namespace Librame.Extensions
{
    /// <summary>
    /// 对象静态扩展。
    /// </summary>
    public static class TypeExtensions
    {
        private static readonly Dictionary<Type, object> _commonTypeDictionary = new Dictionary<Type, object>
        {
            #pragma warning disable IDE0034 // Simplify 'default' expression - default causes default(object)
            { typeof(char), default(char) },
            { typeof(sbyte), default(sbyte) },
            { typeof(short), default(short) },
            { typeof(int), default(int) },
            { typeof(long), default(long) },
            { typeof(byte), default(byte) },
            { typeof(ushort), default(ushort) },
            { typeof(uint), default(uint) },
            { typeof(ulong), default(ulong) },
            { typeof(double), default(double) },
            { typeof(float), default(float) },
            { typeof(bool), default(bool) },
            { typeof(DateTime), default(DateTime) },
            { typeof(DateTimeOffset), default(DateTimeOffset) },
            { typeof(Guid), default(Guid) }
            #pragma warning restore IDE0034 // Simplify 'default' expression
        };


        /// <summary>
        /// 获取默认值。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回对象 NULL（非值类型）或默认值（值类型）。</returns>
        public static object AsDefaultValue(this Type type)
        {
            if (type.IsDefault() || !type.IsValueType) return null;

            // A bit of perf code to avoid calling Activator.CreateInstance for common types and
            // to avoid boxing on every call. This is about 50% faster than just calling CreateInstance
            // for all value types.
            return _commonTypeDictionary.TryGetValue(type, out var value)
                ? value
                : Activator.CreateInstance(type);
        }


        /// <summary>
        /// 打开可空类型。
        /// </summary>
        /// <param name="nullableType">给定的可空类型。</param>
        /// <returns>返回基础类型或可空类型本身。</returns>
        public static Type UnwrapNullableType(this Type nullableType)
        {
            return Nullable.GetUnderlyingType(nullableType) ?? nullableType;
        }


        /// <summary>
        /// 填入属性集合。
        /// </summary>
        /// <param name="source">给定的来源类型实例。</param>
        /// <param name="target">给定的目标类型实例。</param>
        public static void PopulateProperties<TSource, TTarget>(this TSource source, TTarget target)
        {
            source.NotDefault(nameof(source));
            target.NotDefault(nameof(target));

            var srcProperties = new List<PropertyInfo>(typeof(TSource).GetProperties());
            var trgtProperties = new List<PropertyInfo>(typeof(TTarget).GetProperties());

            for (var s = 0; s < srcProperties.Count; s++)
            {
                for (var t = 0; t < trgtProperties.Count; t++)
                {
                    var srcProperty = srcProperties[s];
                    var trgtProperty = trgtProperties[t];

                    if (srcProperty.Name == trgtProperty.Name)
                    {
                        var value = srcProperty.GetValue(source);
                        trgtProperty.SetValue(target, value);

                        trgtProperties.Remove(trgtProperty);
                        srcProperties.Remove(srcProperty);
                        
                        break;
                    }
                }
            }
        }

    }
}
