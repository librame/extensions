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
using System.IO;
using System.Reflection;

namespace Librame.Extensions
{
    /// <summary>
    /// 异常静态扩展。
    /// </summary>
    public static class ExceptionExtensions
    {

        /// <summary>
        /// 得到内部异常消息。
        /// </summary>
        /// <param name="ex">给定的异常。</param>
        /// <returns>返回消息字符串。</returns>
        public static string AsInnerMessage(this Exception ex)
        {
            if (ex.InnerException.IsNotDefault())
                return ex.InnerException.AsInnerMessage();

            return ex.Message;
        }


        /// <summary>
        /// 得到不为 DEFAULT（值类型）或 NULL（引用类型）的类型实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// item 为 DEFAULT（值类型）或 NULL（引用类型）。
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的实例。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回类型实例或抛出异常。</returns>
        public static T NotDefault<T>(this T item, string paramName)
        {
            if (item.IsDefault())
                throw new ArgumentNullException(paramName);

            return item;
        }


        /// <summary>
        /// 得到不为 DEFAULT、空字符串或仅由空字符组成的字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str 为 DEFAULT、空字符串或仅由空字符组成。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回字符串或抛出异常。</returns>
        public static string NotWhiteSpace(this string str, string paramName)
        {
            if (str.IsWhiteSpace())
                throw new ArgumentNullException(paramName);

            return str;
        }


        /// <summary>
        /// 得到不为 NULL 或 EMPTY 的字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str 为 DEFAULT 或空字符串。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回字符串或抛出异常。</returns>
        public static string NotEmpty(this string str, string paramName)
        {
            if (str.IsEmpty())
                throw new ArgumentNullException(paramName);

            return str;
        }


        /// <summary>
        /// 得到不为 NULL 或 EMPTY 的集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// items 为 DEFAULT 或空集合。
        /// </exception>
        /// <typeparam name="T">指定的元素类型。</typeparam>
        /// <param name="items">给定的元素集合。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回集合或抛出异常。</returns>
        public static IEnumerable<T> NotEmpty<T>(this IEnumerable<T> items, string paramName)
        {
            if (items.IsDefault())
                throw new ArgumentNullException(paramName);

            return items;
        }

        /// <summary>
        /// 得到不为 NULL 或 EMPTY 的列表。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// items 为 DEFAULT 或空列表。
        /// </exception>
        /// <typeparam name="T">指定的元素类型。</typeparam>
        /// <param name="items">给定的元素集合。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回集合或抛出异常。</returns>
        public static IList<T> NotEmpty<T>(this IList<T> items, string paramName)
        {
            if (items.IsEmpty())
                throw new ArgumentNullException(paramName);

            return items;
        }

        /// <summary>
        /// 得到不为 NULL 或 EMPTY 的数组。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// items 为 DEFAULT 或空数组。
        /// </exception>
        /// <typeparam name="T">指定的元素类型。</typeparam>
        /// <param name="items">给定的元素集合。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回集合或抛出异常。</returns>
        public static T[] NotEmpty<T>(this T[] items, string paramName)
        {
            if (items.IsEmpty())
                throw new ArgumentNullException(paramName);

            return items;
        }


        /// <summary>
        /// 得到不大于（等于）对比值。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// value is (equal or) greater than.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回值或抛出异常。</returns>
        public static T NotGreater<T>(this T value, T compare, string paramName, bool equals = false)
            where T : IComparable<T>
        {
            if (value.IsGreater(compare, equals))
            {
                var message = $"The \"{paramName}\" value \"{value}\" is (equal or) greater than \"{compare}\".";
                throw new ArgumentOutOfRangeException(paramName, message);
            }

            return value;
        }

        /// <summary>
        /// 得到不小于（等于）的值。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// value is (equal or) lesser than.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回值或抛出异常。</returns>
        public static T NotLesser<T>(this T value, T compare, string paramName, bool equals = false)
            where T : IComparable<T>
        {
            if (value.IsLesser(compare, equals))
            {
                var message = $"The \"{paramName}\" value \"{value}\" is (equal or) lesser than \"{compare}\".";
                throw new ArgumentOutOfRangeException(paramName, message);
            }

            return value;
        }

        /// <summary>
        /// 得到不超出范围的值。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// value is out of range.
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        /// <returns>返回值或抛出异常。</returns>
        public static T NotOutOfRange<T>(this T value, T compareMinimum, T compareMaximum, string paramName,
            bool equalMinimum = false, bool equalMaximum = false)
            where T : IComparable<T>
        {
            if (value.IsOutOfRange(compareMinimum, compareMaximum, equalMinimum, equalMaximum))
            {
                var message = $"The \"{paramName}\" value \"{value}\" is out of range (min: \"{compareMinimum}\", max: \"{compareMaximum}\").";
                throw new ArgumentOutOfRangeException(paramName, message);
            }

            return value;
        }


        /// <summary>
        /// 得到文件存在的路径。
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// path 文件不存在。
        /// </exception>
        /// <param name="path">给定的文件路径。</param>
        /// <returns>返回路径字符串或抛出异常。</returns>
        public static string FileExists(this string path)
        {
            if (!File.Exists(path))
                throw new FileNotFoundException(path);

            return path;
        }

        /// <summary>
        /// 得到目录存在的路径。
        /// </summary>
        /// <exception cref="FileNotFoundException">
        /// path 目录不存在。
        /// </exception>
        /// <param name="path">给定的目录路径。</param>
        /// <returns>返回路径字符串或抛出异常。</returns>
        public static string DirectoryExists(this string path)
        {
            if (!Directory.Exists(path))
                throw new DirectoryNotFoundException(path);

            return path;
        }


        /// <summary>
        /// 基础类型能从指定类型中派生（返回派生类型）。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// baseType 类型不能从 fromType 类型派生。
        /// </exception>
        /// <typeparam name="TBase">指定的基础类型。</typeparam>
        /// <typeparam name="TFrom">指定的派生类型。</typeparam>
        /// <returns>返回派生类型或抛出异常。</returns>
        public static Type CanAssignableFromType<TBase, TFrom>()
        {
            return typeof(TBase).CanAssignableFromType(typeof(TFrom));
        }
        /// <summary>
        /// 基础类型能从指定类型中派生（返回派生类型）。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// baseType 或 fromType 为空。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// baseType 类型不能从 fromType 类型派生。
        /// </exception>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="fromType">给定的派生类型。</param>
        /// <returns>返回派生类型或抛出异常。</returns>
        public static Type CanAssignableFromType(this Type baseType, Type fromType)
        {
            baseType.NotDefault(nameof(baseType));
            fromType.NotDefault(nameof(fromType));
            
            var baseTypeInfo = baseType.GetTypeInfo();
            var fromTypeInfo = fromType.GetTypeInfo();

            // 如果是泛型且未包含参数
            if (baseTypeInfo.IsGenericType && baseTypeInfo.GenericTypeParameters.Length > 0)
            {
                // 取得泛型类型参数集合
                var parameters = baseTypeInfo.GenericTypeParameters;

                baseTypeInfo = baseType.MakeGenericType(parameters).GetTypeInfo();
                fromTypeInfo = fromType.MakeGenericType(parameters).GetTypeInfo();
            }
            
            if (!baseTypeInfo.IsAssignableFrom(fromTypeInfo))
            {
                var message = $"The \"{baseType.ToString()}\" is not assignable from \"{fromType.ToString()}\".";
                throw new ArgumentException(message);
            }

            return fromType;
        }


        /// <summary>
        /// 对象是指定类型。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// obj 为空。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// obj 类型不是指定的类型。
        /// </exception>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="obj">给定的对象。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回实例或抛出异常。</returns>
        public static T SameType<T>(this object obj, string paramName)
        {
            obj = obj.NotDefault(paramName);

            if (!(obj is T instance))
            {
                var message = $"The \"{obj.GetType().FullName}\" is not require type \"{typeof(T).FullName}\".";
                throw new ArgumentException(message);
            }

            return instance;
        }

    }
}
