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
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Librame.Extensions
{
    using Resources;

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
            if (ex.IsNull())
                return null;

            if (ex.InnerException.IsNotNull())
                return ex.InnerException.AsInnerMessage();

            return ex.Message;
        }


        #region NotNull

        /// <summary>
        /// 得到不为 NULL 或默认值。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为空或默认值。
        /// </exception>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="source">给定的源。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回源或抛出异常。</returns>
        public static TSource NotNull<TSource>(this TSource source, string paramName)
            => source.IsNotNull() ? source : throw new ArgumentNullException(paramName);


        /// <summary>
        /// 得到不为 NULL 或空格的字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空或空字符串。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回字符串或抛出异常。</returns>
        public static string NotWhiteSpace(this string str, string paramName)
            => str.IsNotWhiteSpace() ? str : throw new ArgumentNullException(paramName);


        /// <summary>
        /// 得到不为 NULL 或空字符串的字符串。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="str"/> 为空或空字符串。
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回字符串或抛出异常。</returns>
        public static string NotEmpty(this string str, string paramName)
            => str.IsNotEmpty() ? str : throw new ArgumentNullException(paramName);

        /// <summary>
        /// 得到不为 NULL 或空集合的集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sources"/> 为空或空集合。
        /// </exception>
        /// <typeparam name="TSources">指定的源集合类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable"/>。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回集合或抛出异常。</returns>
        public static TSources NotEmpty<TSources>(this TSources sources, string paramName)
            where TSources : IEnumerable
            => sources.IsNotEmpty() ? sources : throw new ArgumentNullException(paramName);

        /// <summary>
        /// 得到不为 NULL 或空集合的集合。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="sources"/> 为空或空集合。
        /// </exception>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回集合或抛出异常。</returns>
        public static IEnumerable<TSource> NotEmpty<TSource>(this IEnumerable<TSource> sources, string paramName)
            => sources.IsNotEmpty() ? sources : throw new ArgumentNullException(paramName);

        #endregion


        #region Compare

        /// <summary>
        /// 得到不大于（等于）对比值。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The '{0}' value '{1}' is (equal or) greater than '{2}'.
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
                throw new ArgumentException(InternalResource.ArgumentExceptionGreaterFormat.Format(paramName, value, compare));

            return value;
        }

        /// <summary>
        /// 得到不小于（等于）的值。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The '{0}' value '{1}' is (equal or) lesser than '{2}'.
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
                throw new ArgumentException(InternalResource.ArgumentExceptionLesserFormat.Format(paramName, value, compare));

            return value;
        }

        /// <summary>
        /// 得到不超出范围的值。
        /// </summary>
        /// <exception cref="ArgumentOutOfRangeException">
        /// The '{0}' value '{1}' is out of range (min: '{2}', max: '{3}').
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
                throw new ArgumentOutOfRangeException(InternalResource.ArgumentOutOfRangeExceptionFormat.Format(paramName, value, compareMinimum, compareMaximum));

            return value;
        }

        #endregion


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
        /// 可从目标类型分配，同时返回目标类型。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 或 <paramref name="targetType"/> 为空。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The base type '{0}' is not assignable from target type '{1}'.
        /// </exception>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="targetType">给定的目标类型。</param>
        /// <returns>返回目标类型或抛出异常。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static Type AssignableFromTarget(this Type baseType, Type targetType)
        {
            if (!baseType.IsAssignableFromTargetType(targetType))
                throw new ArgumentException(InternalResource.ArgumentExceptionAssignableFromFormat.Format(baseType.GetSimpleFullName()), targetType.GetSimpleFullName());

            return targetType;
        }

        /// <summary>
        /// 可以分配给基础类型，同时返回目标类型。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="targetType"/> 或 <paramref name="baseType"/> 为空。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="targetType"/> 不能分配给 <paramref name="baseType"/>。
        /// </exception>
        /// <param name="targetType">给定的目标类型。</param>
        /// <param name="baseType">给定的基础类型。</param>
        /// <returns>返回目标类型或抛出异常。</returns>
        public static Type AssignableToBase(this Type targetType, Type baseType)
            => baseType.AssignableFromTarget(targetType);


        /// <summary>
        /// 将来源类型实例转换为目标类型实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="source"/> 为空。
        /// </exception>
        /// <exception cref="ArgumentException">
        /// The source type '{0}' cannot be cast to target type '{1}'.
        /// </exception>
        /// <typeparam name="TSource">指定的来源类型。</typeparam>
        /// <typeparam name="TTarget">指定的目标类型。</typeparam>
        /// <param name="source">给定的来源实例。</param>
        /// <param name="paramName">给定的参数名。</param>
        /// <returns>返回目标类型实例或抛出异常。</returns>
        public static TTarget CastTo<TSource, TTarget>(this TSource source, string paramName)
        {
            source.NotNull(paramName);

            if (!(source is TTarget target))
                throw new ArgumentException(InternalResource.ArgumentExceptionCastToFormat.Format(source.GetType().GetSimpleFullName(), typeof(TTarget).GetSimpleFullName()));

            return target;
        }

    }
}
