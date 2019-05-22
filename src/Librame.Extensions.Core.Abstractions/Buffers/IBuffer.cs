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
    /// 缓冲区接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IBuffer<T> : IEquatable<IBuffer<T>>
    {
        /// <summary>
        /// 存储器。
        /// </summary>
        /// <value>返回 <see cref="Memory{T}"/>。</value>
        Memory<T> Memory { get; set; }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        IBuffer<T> Copy();
    }
}
