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
    /// 缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class Buffer<T> : ReadOnlyBuffer<T>, IBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="Buffer{T}"/> 实例。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        public Buffer(ReadOnlyMemory<T> readOnlyMemory)
            : base(readOnlyMemory)
        {
            Memory = readOnlyMemory.ToArray();
        }

        /// <summary>
        /// 构造一个 <see cref="Buffer{T}"/> 实例。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{T}"/>。</param>
        public Buffer(Memory<T> memory)
            : base(memory)
        {
            Memory = memory;
        }


        /// <summary>
        /// 存储器。
        /// </summary>
        /// <value>返回 <see cref="Memory{T}"/>。</value>
        public Memory<T> Memory { get; set; }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IBuffer<T> Copy()
        {
            return new Buffer<T>(Memory);
        }


        /// <summary>
        /// 是否相等（默认不比较只读缓冲区）。
        /// </summary>
        /// <param name="other">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        public virtual bool Equals(IBuffer<T> other)
        {
            return Memory.Equals(other.Memory);
        }
        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="compareReadOnly">是否同时比较只读缓冲区。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        public virtual bool Equals(IBuffer<T> other, bool compareReadOnly)
        {
            if (compareReadOnly)
                return Equals(other) && base.Equals(other);

            return Equals(other);
        }

        /// <summary>
        /// 是否相等（默认不比较只读缓冲区）。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        public override bool Equals(object obj)
        {
            if (obj is IBuffer<T> other)
                return Equals(other);

            return false;
        }
        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <param name="compareReadOnly">是否同时比较只读缓冲区。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        public virtual bool Equals(object obj, bool compareReadOnly)
        {
            if (!(obj is IBuffer<T> other))
                return false;

            if (compareReadOnly)
                return Equals(other) && base.Equals(other);

            return Equals(other);
        }


        /// <summary>
        /// 获取哈希码（默认不加入只读缓冲区）。
        /// </summary>
        /// <returns>返回 <see cref="int"/>。</returns>
        public override int GetHashCode()
        {
            return Memory.GetHashCode();
        }
        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <param name="andReadOnly">是否加入只读缓冲区。</param>
        /// <returns>返回 <see cref="int"/>。</returns>
        public virtual int GetHashCode(bool andReadOnly)
        {
            if (andReadOnly)
                return GetHashCode() ^ base.GetHashCode();

            return GetHashCode();
        }

    }
}
