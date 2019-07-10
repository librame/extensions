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
using System.Reflection;

namespace Librame.Extensions
{
    /// <summary>
    /// 对象静态扩展。
    /// </summary>
    public static class TypeExtensions
    {
        /// <summary>
        /// 字符串类型。
        /// </summary>
        public static readonly Type StringType = typeof(string);

        /// <summary>
        /// 可空类型。
        /// </summary>
        public static readonly Type NullableType = typeof(Nullable<>);


        /// <summary>
        /// 常用标记（包括公开、非公开、实例、静态等）。
        /// </summary>
        public static readonly BindingFlags CommonFlags
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static;

        /// <summary>
        /// 常用非静态标记（包括公开、非公开、实例等）。
        /// </summary>
        public static readonly BindingFlags CommonFlagsWithoutStatic
            = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance;

        //private static readonly Dictionary<Type, object> _commonTypeDictionary = new Dictionary<Type, object>
        //{
        //    #pragma warning disable IDE0034 // Simplify 'default' expression - default causes default(object)
        //    { typeof(char), default(char) },
        //    { typeof(sbyte), default(sbyte) },
        //    { typeof(short), default(short) },
        //    { typeof(int), default(int) },
        //    { typeof(long), default(long) },
        //    { typeof(byte), default(byte) },
        //    { typeof(ushort), default(ushort) },
        //    { typeof(uint), default(uint) },
        //    { typeof(ulong), default(ulong) },
        //    { typeof(double), default(double) },
        //    { typeof(float), default(float) },
        //    { typeof(bool), default(bool) },
        //    { typeof(DateTime), default(DateTime) },
        //    { typeof(DateTimeOffset), default(DateTimeOffset) },
        //    { typeof(Guid), default(Guid) }
        //    #pragma warning restore IDE0034 // Simplify 'default' expression
        //};

        ///// <summary>
        ///// 创建实例（引用类型）或默认值（值类型）。
        ///// </summary>
        ///// <param name="type">给定的类型。</param>
        ///// <returns>返回对象。</returns>
        //public static object CreateOrDefault(this Type type)
        //{
        //    type.NotNull(nameof(type));

        //    // A bit of perf code to avoid calling Activator.CreateInstance for common types and
        //    // to avoid boxing on every call. This is about 50% faster than just calling CreateInstance
        //    // for all value types.
        //    return _commonTypeDictionary.TryGetValue(type, out var value)
        //        ? value : type.EnsureCreate();
        //}


        /// <summary>
        /// 获取所有字段集合（包括公开、非公开、实例、静态等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        public static FieldInfo[] GetAllFields(this Type type)
        {
            return type.GetFields(CommonFlags);
        }
        /// <summary>
        /// 获取所有非静态字段集合（包括公开、非公开、实例等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        public static FieldInfo[] GetAllFieldsWithoutStatic(this Type type)
        {
            return type.GetFields(CommonFlagsWithoutStatic);
        }

        /// <summary>
        /// 获取所有属性集合（包括公开、非公开、实例、静态等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        public static PropertyInfo[] GetAllProperties(this Type type)
        {
            return type.GetProperties(CommonFlags);
        }
        /// <summary>
        /// 获取所有非静态属性集合（包括公开、非公开、实例等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        public static PropertyInfo[] GetAllPropertiesWithoutStatic(this Type type)
        {
            return type.GetProperties(CommonFlagsWithoutStatic);
        }


        /// <summary>
        /// 解开可空类型。
        /// </summary>
        /// <param name="nullableType">给定的可空类型。</param>
        /// <returns>返回基础类型或可空类型本身。</returns>
        public static Type UnwrapNullableType(this Type nullableType)
        {
            return Nullable.GetUnderlyingType(nullableType) ?? nullableType;
        }

    }
}
