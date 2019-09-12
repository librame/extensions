﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象只读的连续内存区域缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public abstract class AbstractReadOnlyMemoryBuffer<T> : AbstractDisposable, IReadOnlyMemoryBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractReadOnlyMemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        public AbstractReadOnlyMemoryBuffer(ReadOnlyMemory<T> readOnlyMemory)
        {
            Memory = readOnlyMemory;
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractReadOnlyMemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public AbstractReadOnlyMemoryBuffer(T[] array)
        {
            Memory = array.NotNull(nameof(array));
        }


        /// <summary>
        /// 只读的连续内存区域。
        /// </summary>
        /// <value>返回 <see cref="ReadOnlyMemory{T}"/>。</value>
        public ReadOnlyMemory<T> Memory { get; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IReadOnlyMemoryBuffer{T}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(IReadOnlyMemoryBuffer<T> other)
        {
            other.NotNull(nameof(other));
            return Memory.ToArray().SequenceEqual(other.Memory.ToArray());
            //return Memory.Equals(other.Memory);
        }

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is IReadOnlyMemoryBuffer<T> other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Memory.GetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Memory.ToString();


        /// <summary>
        /// 释放核心对象。
        /// </summary>
        protected override void DisposeCore()
            => Memory.Pin().Dispose();


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractReadOnlyMemoryBuffer{T}"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractReadOnlyMemoryBuffer{T}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(AbstractReadOnlyMemoryBuffer<T> a, AbstractReadOnlyMemoryBuffer<T> b)
            => a.Equals(b);

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractReadOnlyMemoryBuffer{T}"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractReadOnlyMemoryBuffer{T}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(AbstractReadOnlyMemoryBuffer<T> a, AbstractReadOnlyMemoryBuffer<T> b)
            => !a.Equals(b);
    }
}