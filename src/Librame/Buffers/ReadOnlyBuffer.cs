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

namespace Librame.Buffers
{
    /// <summary>
    /// 只读缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class ReadOnlyBuffer<T> : IReadOnlyBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="ReadOnlyBuffer{T}"/> 实例。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        public ReadOnlyBuffer(ReadOnlyMemory<T> readOnlyMemory)
        {
            ReadOnlyMemory = readOnlyMemory;
        }


        /// <summary>
        /// 只读存储器。
        /// </summary>
        /// <value>返回 <see cref="ReadOnlyMemory{T}"/>。</value>
        public ReadOnlyMemory<T> ReadOnlyMemory { get; }


        /// <summary>
        /// 创建只读副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyBuffer{T}"/>。</returns>
        public virtual IReadOnlyBuffer<T> CopyReadOnly()
        {
            return new ReadOnlyBuffer<T>(ReadOnlyMemory);
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IReadOnlyBuffer{T}"/>。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        public virtual bool Equals(IReadOnlyBuffer<T> other)
        {
            return ReadOnlyMemory.Equals(other.ReadOnlyMemory);
        }

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        public override bool Equals(object obj)
        {
            if (obj is IReadOnlyBuffer<T> other)
                return Equals(other);

            return false;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 <see cref="int"/>。</returns>
        public override int GetHashCode()
        {
            return ReadOnlyMemory.GetHashCode();
        }

    }
}
