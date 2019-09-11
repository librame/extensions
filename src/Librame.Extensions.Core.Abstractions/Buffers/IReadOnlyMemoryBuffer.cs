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
    /// 只读的连续内存缓冲区接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IReadOnlyMemoryBuffer<T> : IEquatable<IReadOnlyMemoryBuffer<T>>, IDisposable
    {
        /// <summary>
        /// 只读的连续内存区域。
        /// </summary>
        /// <value>返回 <see cref="ReadOnlyMemory{T}"/>。</value>
        ReadOnlyMemory<T> Memory { get; }
    }
}
