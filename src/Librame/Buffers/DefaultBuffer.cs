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
    /// 默认缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class DefaultBuffer<T> : DefaultReadOnlyBuffer<T>, IBuffer<T>
    {
        /// <summary>
        /// 存储器。
        /// </summary>
        public new Memory<T> Memory { get; protected set; }
            = Memory<T>.Empty;


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual new IBuffer<T> Copy()
        {
            return new DefaultBuffer<T>().ChangeMemory(Memory);
        }


        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <param name="changeFactory">给定改变存储器的工厂方法。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IBuffer<T> ChangeMemory(Func<Memory<T>, ReadOnlyMemory<T>> changeFactory)
        {
            return ChangeMemory(changeFactory.Invoke(Memory));
        }

        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IBuffer<T> ChangeMemory(ReadOnlyMemory<T> readOnlyMemory)
        {
            base.Memory = readOnlyMemory;

            // Clear Memory
            Memory = Memory<T>.Empty;
            base.Memory.CopyTo(Memory);

            return this;
        }

        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <param name="changeFactory">给定改变存储器的工厂方法。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IBuffer<T> ChangeMemory(Func<Memory<T>, Memory<T>> changeFactory)
        {
            return ChangeMemory(changeFactory.Invoke(Memory));
        }

        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{T}"/>。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IBuffer<T> ChangeMemory(Memory<T> memory)
        {
            Memory = memory;
            base.Memory = memory;

            return this;
        }


        /// <summary>
        /// 清空存储器。
        /// </summary>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public virtual IBuffer<T> ClearMemory()
        {
            Memory = Memory<T>.Empty;
            base.Memory = ReadOnlyMemory<T>.Empty;

            return this;
        }

    }
}
