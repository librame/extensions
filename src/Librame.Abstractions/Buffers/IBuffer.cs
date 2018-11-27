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
    public interface IBuffer<T> : IReadOnlyBuffer<T>, IEquatable<IBuffer<T>>
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

        
        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <param name="compareReadOnly">是否同时比较只读缓冲区。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        bool Equals(IBuffer<T> other, bool compareReadOnly);
        
        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <param name="compareReadOnly">是否同时比较只读缓冲区。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        bool Equals(object obj, bool compareReadOnly);

        
        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <param name="andReadOnly">是否加入只读缓冲区。</param>
        /// <returns>返回 <see cref="int"/>。</returns>
        int GetHashCode(bool andReadOnly);
    }
}
