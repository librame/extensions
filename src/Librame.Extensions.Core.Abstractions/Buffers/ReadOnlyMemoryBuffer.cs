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
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 只读的连续内存区域缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class ReadOnlyMemoryBuffer<T> : AbstractDisposable, IReadOnlyMemoryBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="ReadOnlyMemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        public ReadOnlyMemoryBuffer(ReadOnlyMemory<T> readOnlyMemory)
        {
            Memory = readOnlyMemory;
        }

        /// <summary>
        /// 构造一个 <see cref="ReadOnlyMemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public ReadOnlyMemoryBuffer(T[] array)
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "other")]
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
        /// <param name="a">给定的 <see cref="ReadOnlyMemoryBuffer{T}"/>。</param>
        /// <param name="b">给定的 <see cref="ReadOnlyMemoryBuffer{T}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(ReadOnlyMemoryBuffer<T> a, ReadOnlyMemoryBuffer<T> b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="ReadOnlyMemoryBuffer{T}"/>。</param>
        /// <param name="b">给定的 <see cref="ReadOnlyMemoryBuffer{T}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(ReadOnlyMemoryBuffer<T> a, ReadOnlyMemoryBuffer<T> b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串形式。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyMemoryBuffer{T}"/>。</param>
        public static implicit operator string(ReadOnlyMemoryBuffer<T> buffer)
            => buffer?.ToString();
    }
}
