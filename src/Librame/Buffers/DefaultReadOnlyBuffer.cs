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
    /// 默认只读缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public class DefaultReadOnlyBuffer<T> : IReadOnlyBuffer<T>
    {
        /// <summary>
        /// 只读存储器。
        /// </summary>
        public ReadOnlyMemory<T> Memory { get; protected set; }
            = ReadOnlyMemory<T>.Empty;


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyBuffer{T}"/>。</returns>
        public virtual IReadOnlyBuffer<T> Copy()
        {
            return new DefaultReadOnlyBuffer<T>().ChangeReadOnlyMemory(Memory);
        }


        /// <summary>
        /// 改变只读存储器。
        /// </summary>
        /// <param name="memory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyBuffer{T}"/>。</returns>
        public IReadOnlyBuffer<T> ChangeReadOnlyMemory(ReadOnlyMemory<T> memory)
        {
            Memory = memory;

            return this;
        }

    }
}
