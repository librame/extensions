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
    /// 只读缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class ReadOnlyBuffer<T> : AbstractReadOnlyBuffer<T>, IReadOnlyBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="ReadOnlyBuffer{T}"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        public ReadOnlyBuffer(ReadOnlyMemory<T> readOnlyMemory)
            : base(readOnlyMemory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ReadOnlyBuffer{T}"/>。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public ReadOnlyBuffer(T[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 创建一个浅副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyBuffer{T}"/>。</returns>
        public override IReadOnlyBuffer<T> ShallowClone()
            => new ReadOnlyBuffer<T>(Memory);

        /// <summary>
        /// 创建一个深副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyBuffer{T}"/>。</returns>
        public override IReadOnlyBuffer<T> DeepClone()
            => ShallowClone(); // ReadOnlyMemory<T> 结构体是值类型，无谓深浅


        /// <summary>
        /// 隐式转换为只读存储器。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyBuffer{T}"/>。</param>
        public static implicit operator ReadOnlyMemory<T>(ReadOnlyBuffer<T> buffer)
            => buffer.Memory;

        /// <summary>
        /// 隐式转换为数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyBuffer{T}"/>。</param>
        public static implicit operator T[](ReadOnlyBuffer<T> buffer)
            => buffer.Memory.ToArray();


        /// <summary>
        /// 显式转换为只读缓冲区。
        /// </summary>
        /// <param name="bytes">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        public static explicit operator ReadOnlyBuffer<T>(ReadOnlyMemory<T> bytes)
            => new ReadOnlyBuffer<T>(bytes);

        /// <summary>
        /// 显式转换为只读缓冲区。
        /// </summary>
        /// <param name="bytes">给定的数组。</param>
        public static explicit operator ReadOnlyBuffer<T>(T[] bytes)
            => new ReadOnlyBuffer<T>(bytes);

    }
}
