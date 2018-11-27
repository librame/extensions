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
    /// 只读缓冲区接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IReadOnlyBuffer<T> : IEquatable<IReadOnlyBuffer<T>>
    {
        /// <summary>
        /// 只读存储器。
        /// </summary>
        /// <value>返回 <see cref="ReadOnlyMemory{T}"/>。</value>
        ReadOnlyMemory<T> ReadOnlyMemory { get; }


        /// <summary>
        /// 创建只读副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyBuffer{T}"/>。</returns>
        IReadOnlyBuffer<T> CopyReadOnly();
    }
}
