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
    /// 缓冲区接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IBuffer<T> : IReadOnlyBuffer<T>
    {
        /// <summary>
        /// 存储器。
        /// </summary>
        new Memory<T> Memory { get; }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        new IBuffer<T> Copy();


        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <param name="changeFactory">给定改变存储器的工厂方法。</param>
        /// <returns>返回 <see cref="IBuffer{TBuffer}"/>。</returns>
        IBuffer<T> ChangeMemory(Func<Memory<T>, ReadOnlyMemory<T>> changeFactory);

        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<T> ChangeMemory(ReadOnlyMemory<T> readOnlyMemory);

        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <param name="changeFactory">给定改变存储器的工厂方法。</param>
        /// <returns>返回 <see cref="IBuffer{TBuffer}"/>。</returns>
        IBuffer<T> ChangeMemory(Func<Memory<T>, Memory<T>> changeFactory);

        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <param name="memory">给定的存储器。</param>
        /// <returns>返回 <see cref="IBuffer{TBuffer}"/>。</returns>
        IBuffer<T> ChangeMemory(Memory<T> memory);


        /// <summary>
        /// 清空存储器。
        /// </summary>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<T> ClearMemory();
    }
}
