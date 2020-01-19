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

namespace Librame.Extensions.Core.Utilities
{
    ///// <summary>
    ///// 相等比较器实用工具。
    ///// </summary>
    //public static class EqualityComparerUtility
    //{
    //    class EqualityComparerFactory<T> : IEqualityComparer<T>
    //    {
    //        private readonly Func<T, T, bool> _comparerFactory;
    //        private readonly Func<T, int> _hashFactory;


    //        public EqualityComparerFactory(Func<T, T, bool> comparerFactory, Func<T, int> hashFactory)
    //        {
    //            _comparerFactory = comparerFactory.NotNull(nameof(comparerFactory));
    //            _hashFactory = hashFactory.NotNull(nameof(hashFactory));
    //        }


    //        public bool Equals(T x, T y)
    //            => _comparerFactory.Invoke(x, y);

    //        public int GetHashCode(T obj)
    //            => _hashFactory.Invoke(obj);
    //    }


    //    /// <summary>
    //    /// 获取相等比较器。
    //    /// </summary>
    //    /// <typeparam name="T">指定的比较类型。</typeparam>
    //    /// <param name="comparerFactory">给定的比较器工厂方法。</param>
    //    /// <param name="hashFactory">给定的哈希工厂方法。</param>
    //    /// <returns>返回 <see cref="IEqualityComparer{T}"/>。</returns>
    //    public static IEqualityComparer<T> GetComparer<T>(Func<T, T, bool> comparerFactory, Func<T, int> hashFactory)
    //        => new EqualityComparerFactory<T>(comparerFactory, hashFactory);

    //    /// <summary>
    //    /// 获取属性相等比较器。
    //    /// </summary>
    //    /// <typeparam name="T">指定的类型。</typeparam>
    //    /// <typeparam name="TProperty">指定的属性类型。</typeparam>
    //    /// <param name="selector">给定的属性选择器。</param>
    //    /// <param name="hashFactory">给定的哈希工厂方法（可选；默认以选择器属性值计算哈希值）。</param>
    //    /// <returns>返回 <see cref="IEqualityComparer{T}"/>。</returns>
    //    public static IEqualityComparer<T> GetPropertyComparer<T, TProperty>(Func<T, TProperty> selector, Func<T, int> hashFactory = null)
    //        where TProperty : IEquatable<TProperty>
    //    {
    //        selector.NotNull(nameof(selector));

    //        return GetComparer((x, y) => selector.Invoke(x).Equals(selector.Invoke(y)),
    //            hashFactory ?? (x => selector.Invoke(x).GetHashCode()));
    //    }

    //    /// <summary>
    //    /// 获取属性相等比较器。
    //    /// </summary>
    //    /// <typeparam name="T">指定的类型。</typeparam>
    //    /// <param name="selector">指定的字符串型属性选择器。</param>
    //    /// <param name="hashFactory">给定的哈希工厂方法（可选；默认以选择器属性值计算哈希值）。</param>
    //    /// <returns>返回 <see cref="IEqualityComparer{T}"/>。</returns>
    //    public static IEqualityComparer<T> GetPropertyComparer<T>(Func<T, string> selector, Func<T, int> hashFactory = null)
    //    {
    //        selector.NotNull(nameof(selector));

    //        return GetComparer((x, y) => selector.Invoke(x).Equals(selector.Invoke(y), StringComparison.OrdinalIgnoreCase),
    //            hashFactory ?? (x => selector.Invoke(x).CompatibleGetHashCode()));
    //    }

    //}
}
