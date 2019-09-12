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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 委托相等比较器。
    /// </summary>
    /// <remarks>https://blog.csdn.net/honantic/article/details/51595823</remarks>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class FuncEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly Func<T, T, bool> _comparerFactory;
        private readonly Func<T, int> _hashFactory;


        /// <summary>
        /// 构造一个 <see cref="FuncEqualityComparer{T}"/>。
        /// </summary>
        /// <param name="comparerFactory">给定的比较器工厂方法。</param>
        public FuncEqualityComparer(Func<T, T, bool> comparerFactory)
            : this(comparerFactory, t => 0) // NB Cannot assume anything about how e.g., t.GetHashCode() interacts with the comparer's behavior
        {
        }

        /// <summary>
        /// 构造一个 <see cref="FuncEqualityComparer{T}"/>。
        /// </summary>
        /// <param name="comparerFactory">给定的比较器工厂方法。</param>
        /// <param name="hashFactory">给定计算哈希的工厂方法。</param>
        public FuncEqualityComparer(Func<T, T, bool> comparerFactory, Func<T, int> hashFactory)
        {
            _comparerFactory = comparerFactory.NotNull(nameof(comparerFactory));
            _hashFactory = hashFactory.NotNull(nameof(hashFactory));
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="x">给定的 <typeparamref name="T"/>。</param>
        /// <param name="y">给定的 <typeparamref name="T"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(T x, T y)
        {
            return _comparerFactory.Invoke(x, y);
        }

        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <param name="obj">给定的 <typeparamref name="T"/>。</param>
        /// <returns>返回整数。</returns>
        public int GetHashCode(T obj)
        {
            return _hashFactory.Invoke(obj);
        }
    }
}