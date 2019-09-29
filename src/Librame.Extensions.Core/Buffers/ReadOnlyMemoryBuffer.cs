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
    /// 只读的连续内存缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class ReadOnlyMemoryBuffer<T> : AbstractReadOnlyMemoryBuffer<T>, IReadOnlyMemoryBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="ReadOnlyMemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        public ReadOnlyMemoryBuffer(ReadOnlyMemory<T> readOnlyMemory)
            : base(readOnlyMemory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ReadOnlyMemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public ReadOnlyMemoryBuffer(T[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 隐式转换为只读内存。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyMemoryBuffer{T}"/>。</param>
        public static implicit operator ReadOnlyMemory<T>(ReadOnlyMemoryBuffer<T> buffer)
            => buffer.Memory;

        /// <summary>
        /// 隐式转换为只读内存缓冲区。
        /// </summary>
        /// <param name="memory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        public static implicit operator ReadOnlyMemoryBuffer<T>(ReadOnlyMemory<T> memory)
            => new ReadOnlyMemoryBuffer<T>(memory);


        /// <summary>
        /// 显式转换为数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyMemoryBuffer{T}"/>。</param>
        public static explicit operator T[](ReadOnlyMemoryBuffer<T> buffer)
            => buffer.Memory.ToArray();

        /// <summary>
        /// 显式转换为只读内存缓冲区。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public static explicit operator ReadOnlyMemoryBuffer<T>(T[] array)
            => new ReadOnlyMemoryBuffer<T>(array);
    }
}
