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
    /// 可读写的连续内存缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class MemoryBuffer<T> : AbstractMemoryBuffer<T>, IMemoryBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="MemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{T}"/>。</param>
        public MemoryBuffer(Memory<T> memory)
            : base(memory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="MemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public MemoryBuffer(T[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 隐式转换为 <see cref="Memory{T}"/>。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="MemoryBuffer{T}"/>。</param>
        public static implicit operator Memory<T>(MemoryBuffer<T> buffer)
            => buffer.Memory;

        /// <summary>
        /// 隐式转换为数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="MemoryBuffer{T}"/>。</param>
        public static implicit operator T[](MemoryBuffer<T> buffer)
            => buffer.Memory.ToArray();


        /// <summary>
        /// 显式转换为 <see cref="MemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="bytes">给定的 <see cref="Memory{T}"/>。</param>
        public static explicit operator MemoryBuffer<T>(Memory<T> bytes)
            => new MemoryBuffer<T>(bytes);

        /// <summary>
        /// 显式转换为 <see cref="MemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="bytes">给定的数组。</param>
        public static explicit operator MemoryBuffer<T>(T[] bytes)
            => new MemoryBuffer<T>(bytes);
    }
}
