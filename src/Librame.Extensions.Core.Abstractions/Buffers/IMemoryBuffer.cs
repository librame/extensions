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
using System.Buffers;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 可读写的连续内存缓冲区接口。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public interface IMemoryBuffer<T> : IMemoryOwner<T>, IEquatable<IMemoryBuffer<T>>
    {
        /// <summary>
        /// 改变 <see cref="Memory{T}"/>。
        /// </summary>
        /// <param name="newMemoryFactory">给定的新 <see cref="Memory{T}"/> 工厂方法。</param>
        /// <returns>返回 <see cref="IMemoryBuffer{T}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "newMemoryFactory")]
        public IMemoryBuffer<T> ChangeMemory(Func<Memory<T>, Memory<T>> newMemoryFactory)
        {
            newMemoryFactory.NotNull(nameof(newMemoryFactory));

            var newMemory = newMemoryFactory.Invoke(Memory);
            return ChangeMemory(newMemory);
        }

        /// <summary>
        /// 改变 <see cref="Memory{T}"/>。
        /// </summary>
        /// <param name="newMemory">给定的新 <see cref="Memory{T}"/>。</param>
        /// <returns>返回 <see cref="IMemoryBuffer{T}"/>。</returns>
        IMemoryBuffer<T> ChangeMemory(Memory<T> newMemory);
    }
}
