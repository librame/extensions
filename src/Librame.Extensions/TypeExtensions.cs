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
using System.Text;

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


        /// <summary>
        /// 获取所有字段集合（包括公开、非公开、实例、静态等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        public static FieldInfo[] GetAllFields(this Type type)
        {
            type.NotNull(nameof(type));

            return type.GetFields(CommonFlags);
        }
        /// <summary>
        /// 获取所有非静态字段集合（包括公开、非公开、实例等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        public static FieldInfo[] GetAllFieldsWithoutStatic(this Type type)
        {
            type.NotNull(nameof(type));

            return type.GetFields(CommonFlagsWithoutStatic);
        }

        /// <summary>
        /// 获取所有属性集合（包括公开、非公开、实例、静态等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        public static PropertyInfo[] GetAllProperties(this Type type)
        {
            type.NotNull(nameof(type));

            return type.GetProperties(CommonFlags);
        }
        /// <summary>
        /// 获取所有非静态属性集合（包括公开、非公开、实例等）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字段信息数组。</returns>
        public static PropertyInfo[] GetAllPropertiesWithoutStatic(this Type type)
        {
            type.NotNull(nameof(type));

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


        /// <summary>
        /// 获取限定名称（格式：AssemblyFullName, TypeFullName）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetQualifiedName(this Type type)
        {
            type.NotNull(nameof(type));

            var assemblyName = type.Assembly.GetName().FullName;

            return Assembly.CreateQualifiedName(assemblyName, type.FullName);
        }


        /// <summary>
        /// 获取主体名称（如泛类型 IDictionary{string, IList{string}} 的主体名称为 IDictionary）。
        /// </summary>
        /// <param name="type">给定的名称。</param>
        /// <returns>返回字符串。</returns>
        public static string GetBodyName(this Type type)
        {
            type.NotNull(nameof(type));

            if (type.IsGenericType)
                return type.Name.SplitPair("`").Key;

            return type.Name;
        }


        /// <summary>
        /// 获取名称（如泛类型 IDictionary{string, IList{string}} 的名称为 IDictionary`2[String, IList`1[String]]）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetName(this Type type)
        {
            return type.GetString(t => t.Name);
        }

        /// <summary>
        /// 获取带命名空间的完整名称（如泛类型 IDictionary{string, IList{string}} 的名称为 System.Collections.Generic.IDictionary`2[System.String, System.Collections.Generic.IList`1[System.String]]）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回字符串。</returns>
        public static string GetFullName(this Type type)
        {
            return type.GetString(t => $"{t.Namespace}.{t.Name}"); // not t.FullName
        }

        /// <summary>
        /// 获取类型字符串。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <param name="factory">给定的类型字符串工厂方法。</param>
        /// <returns>返回字符串。</returns>
        public static string GetString(this Type type, Func<Type, string> factory)
        {
            type.NotNull(nameof(type));

            var sb = new StringBuilder();
            sb.Append(factory.Invoke(type));

            if (type.IsGenericType)
            {
                var argumentTypes = type.GetGenericArguments();
                sb.Append("[");

                argumentTypes.ForEach((argType, i) =>
                {
                    sb.Append(argType.GetString(factory));

                    if (i < argumentTypes.Length - 1)
                        sb.Append(", ");
                });

                sb.Append("]");
            }

            return sb.ToString();
        }

    }
}
