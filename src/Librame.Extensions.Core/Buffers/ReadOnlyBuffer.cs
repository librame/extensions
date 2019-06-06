﻿#region License

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
    public class ReadOnlyBuffer<T> : AbstractReadOnlyBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="ReadOnlyBuffer{T}"/> 实例。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{T}"/>。</param>
        public ReadOnlyBuffer(ReadOnlyMemory<T> readOnlyMemory)
            : base(readOnlyMemory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ReadOnlyBuffer{T}"/> 实例。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public ReadOnlyBuffer(T[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 创建只读副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyBuffer{T}"/>。</returns>
        public override IReadOnlyBuffer<T> Copy()
        {
            return new ReadOnlyBuffer<T>(Memory);
        }

    }
}