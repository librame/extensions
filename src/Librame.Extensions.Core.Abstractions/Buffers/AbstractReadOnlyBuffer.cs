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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象只读缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public abstract class AbstractReadOnlyBuffer<T> : AbstractCloneable<IReadOnlyBuffer<T>>, IReadOnlyBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractReadOnlyBuffer{T}"/> 实例。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        public AbstractReadOnlyBuffer(ReadOnlyMemory<T> readOnlyMemory)
        {
            Memory = readOnlyMemory;
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractReadOnlyBuffer{T}"/> 实例。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public AbstractReadOnlyBuffer(T[] array)
        {
            Memory = array;
        }


        /// <summary>
        /// 存储器。
        /// </summary>
        /// <value>返回 <see cref="ReadOnlyMemory{T}"/>。</value>
        public ReadOnlyMemory<T> Memory { get; }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IReadOnlyBuffer{T}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public virtual bool Equals(IReadOnlyBuffer<T> other)
        {
            other.NotNull(nameof(other));

            return Memory.Equals(other.Memory);
        }

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
        {
            if (obj is IReadOnlyBuffer<T> other)
                return Equals(other);

            return false;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
        {
            return Memory.GetHashCode();
        }


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return Memory.ToString();
        }

    }
}
