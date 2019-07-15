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
    /// 缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class Buffer<T> : AbstractBuffer<T>, IBuffer<T>
    {
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
        /// 构造一个 <see cref="Buffer{T}"/> 实例。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public Buffer(T[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 创建一个浅副本。
        /// </summary>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public override IBuffer<T> ShallowClone()
        {
            return new Buffer<T>(Memory);
        }

        /// <summary>
        /// 创建一个深副本。
        /// </summary>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public override IBuffer<T> DeepClone()
        {
            // ReadOnlyMemory<T> 结构体是值类型，无谓深浅
            return ShallowClone();
        }

    }
}
