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

namespace Librame.Utility
{
    /// <summary>
    /// 转换助手。
    /// </summary>
    public class ConversionHelper
    {
        /// <summary>
        /// 转换类型或返回默认值。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="source">给定的源对象。</param>
        /// <param name="convertFactory">给定的转换方法。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回结果对象。</returns>
        public static TResult AsOrDefault<TSource, TResult>(TSource source, Func<TSource, TResult> convertFactory,
            TResult defaultValue = default(TResult))
        {
            if (ReferenceEquals(source, default(TSource)) || ReferenceEquals(convertFactory, null))
                return defaultValue;

            try
            {
                return convertFactory(source);
            }
            catch
            {
                return defaultValue;
            }
        }


        /// <summary>
        /// 取不为空的当前值或默认值。
        /// </summary>
        /// <param name="value">给定的当前值。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回值。</returns>
        public static T AsOrDefault<T>(T value, T defaultValue)
            where T : class
        {
            bool isNull;
            return AsOrDefault(value, defaultValue, out isNull);
        }
        /// <summary>
        /// 取不为空的当前值或默认值。
        /// </summary>
        /// <param name="value">给定的当前值。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <param name="isNull">输出是否为空的布尔值。</param>
        /// <returns>返回值。</returns>
        public static T AsOrDefault<T>(T value, T defaultValue, out bool isNull)
            where T : class
        {
            isNull = ReferenceEquals(value, null);

            if (isNull)
                return defaultValue;

            return value;
        }


        /// <summary>
        /// 将类型实例转换为键值对字符串形式。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的实例。</param>
        /// <param name="searchMode">给定的搜索模式（可选；默认不进行关键字筛选）。</param>
        /// <param name="searchKeys">要排除的键名集合。</param>
        /// <returns>返回字符串。</returns>
        public static string AsString<T>(T item, SearchMode searchMode = SearchMode.Default, params string[] searchKeys)
        {
            item.GuardNull(nameof(item));

            var pis = typeof(T).GetProperties();

            if (searchKeys.Length > 0)
            {
                if (searchMode == SearchMode.Exclude)
                {
                    foreach (var key in searchKeys)
                    {
                        // 越来越少
                        pis = pis.Where(f => f.Name != key).ToArray();
                    }
                }
                else if (searchMode == SearchMode.Include)
                {
                    var allFiles = new List<PropertyInfo>();

                    foreach (var key in searchKeys)
                    {
                        var keyFiles = pis.Where(f => f.Name == key);

                        allFiles.AddRange(keyFiles);
                    }

                    // 移除可能存在的重复项
                    pis = allFiles.Distinct().ToArray();
                }
                else
                {
                    // 默认不操作
                }
            }

            if (pis.Length > 0)
            {
                var sb = new System.Text.StringBuilder();
                var lastPi = pis[pis.Length - 1];

                foreach (var pi in pis)
                {
                    sb.AppendFormat("{0}={1}", pi.Name, pi.GetValue(item, null));

                    if (pi.Name != lastPi.Name)
                        sb.Append(",");
                }

                return sb.ToString();
            }

            return string.Empty;
        }

    }


    /// <summary>
    /// <see cref="ConversionHelper"/> 静态扩展。
    /// </summary>
    public static class ConversionHelperExtensions
    {
        /// <summary>
        /// 转换类型或返回默认值。
        /// </summary>
        /// <typeparam name="TSource">指定的源类型。</typeparam>
        /// <typeparam name="TResult">指定的结果类型。</typeparam>
        /// <param name="source">给定的源对象。</param>
        /// <param name="convertFactory">给定的转换方法。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回结果对象。</returns>
        public static TResult AsOrDefault<TSource, TResult>(this TSource source, Func<TSource, TResult> convertFactory,
            TResult defaultValue = default(TResult))
        {
            return ConversionHelper.AsOrDefault(source, convertFactory, defaultValue);
        }


        /// <summary>
        /// 取不为空的当前值或默认值。
        /// </summary>
        /// <param name="value">给定的当前值。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <returns>返回值。</returns>
        public static T AsOrDefault<T>(this T value, T defaultValue)
            where T : class
        {
            return ConversionHelper.AsOrDefault(value, defaultValue);
        }
        /// <summary>
        /// 取不为空的当前值或默认值。
        /// </summary>
        /// <param name="value">给定的当前值。</param>
        /// <param name="defaultValue">给定的默认值。</param>
        /// <param name="isNull">输出是否为空的布尔值。</param>
        /// <returns>返回值。</returns>
        public static T AsOrDefault<T>(this T value, T defaultValue, out bool isNull)
            where T : class
        {
            return ConversionHelper.AsOrDefault(value, defaultValue, out isNull);
        }


        /// <summary>
        /// 将类型实例转换为键值对字符串形式。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的实例。</param>
        /// <param name="searchMode">给定的搜索模式（可选；默认不进行关键字筛选）。</param>
        /// <param name="searchKeys">要排除的键名集合。</param>
        /// <returns>返回字符串。</returns>
        public static string AsString<T>(this T item, SearchMode searchMode = SearchMode.Default, params string[] searchKeys)
        {
            return ConversionHelper.AsString(item, searchMode, searchKeys);
        }

    }
}
