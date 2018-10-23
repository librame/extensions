#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace Librame.Extensions
{
    /// <summary>
    /// 可枚举静态扩展。
    /// </summary>
    public static class EnumerableExtensions
    {

        /// <summary>
        /// 迭代为可枚举集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="instance">给定的类型实例。</param>
        /// <returns>返回可枚举接口。</returns>
        public static IEnumerable<T> YieldEnumerable<T>(this T instance)
        {
            yield return instance;
        }


        /// <summary>
        /// 转换为只读列表集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="enumerable">给定的 <see cref="IEnumerable{T}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{T}"/>。</returns>
        public static IReadOnlyList<T> AsReadOnlyList<T>(this IEnumerable<T> enumerable)
        {
            return new ReadOnlyCollection<T>(enumerable.ToList());
        }

        /// <summary>
        /// 转换为只读列表集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="list">给定的 <see cref="IList{T}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyList{T}"/>。</returns>
        public static IReadOnlyList<T> AsReadOnlyList<T>(this IList<T> list)
        {
            return new ReadOnlyCollection<T>(list);
        }

    }
}
