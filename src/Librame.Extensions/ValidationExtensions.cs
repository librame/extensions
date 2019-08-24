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
using System.Linq;
using System.Reflection;

namespace Librame.Extensions
{
    /// <summary>
    /// 验证静态扩展。
    /// </summary>
    public static class ValidationExtensions
    {

        #region NullOrEmpty

        /// <summary>
        /// 是否为 NULL。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="source">给定的源实例。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNull<TSource>(this TSource source)
        {
            return null == source;
        }

        /// <summary>
        /// 是否不为 NULL。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="source">给定的源实例。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotNull<TSource>(this TSource source)
        {
            return null != source;
        }


        /// <summary>
        /// 是否为 NULL 或空格。
        /// </summary>
        /// <remarks>
        /// 详情参考 <see cref="string.IsNullOrWhiteSpace(string)"/>。
        /// </remarks>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNullOrWhiteSpace(this string str)
        {
            return string.IsNullOrWhiteSpace(str);
        }

        /// <summary>
        /// 是否不为 NULL 或空格。
        /// </summary>
        /// <remarks>
        /// 详情参考 <see cref="string.IsNullOrWhiteSpace(string)"/>。
        /// </remarks>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotNullOrWhiteSpace(this string str)
        {
            return !string.IsNullOrWhiteSpace(str);
        }


        /// <summary>
        /// 是否为 NULL 或空字符串。
        /// </summary>
        /// <remarks>
        /// 详情参考 <see cref="string.IsNullOrEmpty(string)"/>。
        /// </remarks>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNullOrEmpty(this string str)
        {
            return string.IsNullOrEmpty(str);
        }

        /// <summary>
        /// 是否不为 NULL 或空字符串。
        /// </summary>
        /// <remarks>
        /// 详情参考 <see cref="string.IsNullOrEmpty(string)"/>。
        /// </remarks>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotNullOrEmpty(this string str)
        {
            return !string.IsNullOrEmpty(str);
        }


        /// <summary>
        /// 是否为 NULL 或空集合。
        /// </summary>
        /// <typeparam name="TSources">指定的源集合类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNullOrEmpty<TSources>(this TSources sources)
            where TSources : IEnumerable
        {
            if (sources.IsNull())
                return true;

            var i = 0;
            foreach (var source in sources)
            {
                if (i > 0)
                    break;
                i++;
            }

            return i < 1;
        }

        /// <summary>
        /// 是否不为 NULL 或空集合。
        /// </summary>
        /// <typeparam name="TSources">指定的源集合类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotNullOrEmpty<TSources>(this TSources sources)
            where TSources : IEnumerable
        {
            return !sources.IsNullOrEmpty();
        }


        /// <summary>
        /// 是否为 NULL 或空集合。
        /// </summary>
        /// <remarks>
        /// 详情参考  <see cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>。
        /// </remarks>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNullOrEmpty<TSource>(this IEnumerable<TSource> sources)
        {
            if (sources.IsNull())
                return true;

            return !sources.Any();
        }

        /// <summary>
        /// 是否不为 NULL 或空集合。
        /// </summary>
        /// <remarks>
        /// 详情参考  <see cref="Enumerable.Any{TSource}(IEnumerable{TSource})"/>。
        /// </remarks>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <param name="sources">给定的 <see cref="IEnumerable{TSource}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNotNullOrEmpty<TSource>(this IEnumerable<TSource> sources)
        {
            return !sources.IsNullOrEmpty();
        }

        #endregion


        #region Compare

        /// <summary>
        /// 是否为倍数。
        /// </summary>
        /// <param name="value">给定的数字。</param>
        /// <param name="multiples">给定的倍数。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsMultiples(this int value, int multiples)
        {
            return 0 == value % multiples;
        }


        /// <summary>
        /// 是否大于或大于等于对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsGreater<T>(this T value, T compare, bool equals = false)
            where T : IComparable<T>
        {
            value.NotNull(nameof(value));

            return equals ? value.CompareTo(compare) >= 0 : value.CompareTo(compare) > 0;
        }
        /// <summary>
        /// 是否小于或小于等于对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compare">给定的比较值。</param>
        /// <param name="equals">是否比较等于（可选；默认不比较）。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLesser<T>(this T value, T compare, bool equals = false)
            where T : IComparable<T>
        {
            value.NotNull(nameof(value));

            return equals ? value.CompareTo(compare) <= 0 : value.CompareTo(compare) < 0;
        }

        /// <summary>
        /// 是否超出或等于范围对比值。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="value">给定的值。</param>
        /// <param name="compareMinimum">给定的最小比较值。</param>
        /// <param name="compareMaximum">给定的最大比较值。</param>
        /// <param name="equalMinimum">是否比较等于最小值（可选；默认不比较）。</param>
        /// <param name="equalMaximum">是否比较等于最大值（可选；默认不比较）。</param>
        public static bool IsOutOfRange<T>(this T value, T compareMinimum, T compareMaximum,
            bool equalMinimum = false, bool equalMaximum = false)
            where T : IComparable<T>
        {
            if (value.IsLesser(compareMinimum, equalMinimum))
                return true;

            return value.IsGreater(compareMaximum, equalMaximum);
        }

        #endregion


        #region Type

        /// <summary>
        /// 是否为具体实类型（非接口与抽象类型，即可实例化类型）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsConcreteType(this Type type)
        {
            return type.IsNotNull()
                && !type.IsAbstract
                && !type.IsInterface;
        }

        /// <summary>
        /// 是否为开放式泛型（泛类型定义或包含泛型参数集合）。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsOpenGenericType(this Type type)
        {
            // 如泛型 List<string>，则 GenericTypeDefinition 为 List<T>，GenericParameters 为 string
            return type.IsNotNull()
                && (type.IsGenericTypeDefinition || type.ContainsGenericParameters);
        }

        /// <summary>
        /// 是否为可空类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsNullableType(this Type type)
        {
            // 如可空泛型 int?，则 GenericTypeDefinition() 为 Nullable<T>
            return type.IsNotNull()
                && type.IsGenericType
                && type.GetGenericTypeDefinition() == TypeExtensions.NullableType;
        }

        /// <summary>
        /// 是否为字符串类型。
        /// </summary>
        /// <param name="type">给定的类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsStringType(this Type type)
        {
            return type.IsNotNull()
                && type == TypeExtensions.StringType;
        }

        /// <summary>
        /// 是否可以从目标类型分配。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 或 <paramref name="targetType"/> 为空。
        /// </exception>
        /// <remarks>
        /// 详情参考 <see cref="Type.IsAssignableFrom(Type)"/>。
        /// </remarks>
        /// <param name="baseType">给定的基础类型。</param>
        /// <param name="targetType">给定的目标类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAssignableFromTargetType(this Type baseType, Type targetType)
        {
            baseType.NotNull(nameof(baseType));
            targetType.NotNull(nameof(targetType));

            var baseTypeInfo = baseType.GetTypeInfo();
            var fromTypeInfo = targetType.GetTypeInfo();

            // 对泛型提供支持
            if (baseTypeInfo.IsGenericType && baseTypeInfo.GenericTypeParameters.Length > 0)
            {
                baseTypeInfo = baseType.MakeGenericType(baseTypeInfo.GenericTypeParameters).GetTypeInfo();
                fromTypeInfo = targetType.MakeGenericType(fromTypeInfo.GenericTypeParameters).GetTypeInfo();
            }

            return baseTypeInfo.IsAssignableFrom(fromTypeInfo);
        }

        /// <summary>
        /// 是否可以分配给基础类型。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="baseType"/> 或 <paramref name="targetType"/> 为空。
        /// </exception>
        /// <remarks>
        /// 与 <see cref="IsAssignableFromTargetType(Type, Type)"/> 参数相反。
        /// </remarks>
        /// <param name="targetType">给定的目标类型。</param>
        /// <param name="baseType">给定的基础类型。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAssignableToBaseType(this Type targetType, Type baseType)
        {
            return baseType.IsAssignableFromTargetType(targetType);
        }

        #endregion


        #region Digit & Letter & Special

        /// <summary>
        /// 是否有数字。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasDigit(this string str)
            => str.Any(IsDigit);

        /// <summary>
        /// 是否为数字。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDigit(this string str)
            => str.All(IsDigit);

        /// <summary>
        /// 是否为数字。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsDigit(this char c)
            => c >= '0' && c <= '9';


        /// <summary>
        /// 是否有小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasLower(this string str)
            => str.Any(IsLower);

        /// <summary>
        /// 是否为小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLower(this string str)
            => str.All(IsLower);

        /// <summary>
        /// 是否为小写字母。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLower(this char c)
            => c >= 'a' && c <= 'z';


        /// <summary>
        /// 是否有大写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasUpper(this string str)
            => str.Any(IsUpper);

        /// <summary>
        /// 是否为大写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsUpper(this string str)
            => str.All(IsUpper);

        /// <summary>
        /// 是否为大写字母。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsUpper(this char c)
            => c >= 'A' && c <= 'Z';


        /// <summary>
        /// 是否有部分特殊符号。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasSpecial(this string str)
            => str.Any(IsSpecial);

        /// <summary>
        /// 是否为部分特殊符号。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsSpecial(this string str)
            => str.All(IsSpecial);

        /// <summary>
        /// 是否为部分特殊符号。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsSpecial(this char c)
            => AlgorithmExtensions.SPECIAL.Contains(c);


        /// <summary>
        /// 是否有大小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasLetter(this string str)
            => str.HasUpper() && str.HasLower();

        /// <summary>
        /// 是否为大小写字母。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetter(this string str)
            => str.HasLetter() && str.All(IsLetter);

        /// <summary>
        /// 是否为大小写字母。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetter(this char c)
        {
            if (c.IsUpper())
                return true;

            return c.IsLower();
        }


        /// <summary>
        /// 是否有大小写字母和数字。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasLetterAndDigit(this string str)
            => str.HasLetter() && str.HasDigit();

        /// <summary>
        /// 是否为大小写字母和数字。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetterAndDigit(this string str)
            => str.HasLetterAndDigit() && str.All(IsLetterAndDigit);

        /// <summary>
        /// 是否为大小写字母和数字。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsLetterAndDigit(this char c)
        {
            if (c.IsLetter())
                return true;

            return c.IsDigit();
        }


        /// <summary>
        /// 是否有大小写字母、数字和部分特殊符号。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool HasSafety(this string str)
            => str.HasLetterAndDigit() && str.HasSpecial();

        /// <summary>
        /// 是否为大小写字母、数字和部分特殊符号。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsSafety(this string str)
            => str.HasSafety() && str.All(IsSafety);

        /// <summary>
        /// 是否为大小写字母、数字和部分特殊符号。
        /// </summary>
        /// <param name="c">给定的字符。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsSafety(this char c)
        {
            if (c.IsLetterAndDigit())
                return true;

            return c.IsSpecial();
        }

        #endregion

    }
}
