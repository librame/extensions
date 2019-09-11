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
using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象可读写的连续内存缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public abstract class AbstractMemoryBuffer<T> : AbstractDisposable, IMemoryBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractMemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{T}"/>。</param>
        public AbstractMemoryBuffer(Memory<T> memory)
        {
            Memory = memory;
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractMemoryBuffer{T}"/>。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public AbstractMemoryBuffer(T[] array)
        {
            Memory = array.NotNull(nameof(array));
        }


        /// <summary>
        /// 可读写的连续内存区域。
        /// </summary>
        /// <value>返回 <see cref="Memory{T}"/>。</value>
        public Memory<T> Memory { get; private set; }


        /// <summary>
        /// 改变 <see cref="Memory{T}"/>。
        /// </summary>
        /// <param name="newMemoryFactory">给定的新 <see cref="Memory{T}"/> 工厂方法。</param>
        /// <returns>返回 <see cref="IMemoryBuffer{T}"/>。</returns>
        public virtual IMemoryBuffer<T> ChangeMemory(Func<Memory<T>, Memory<T>> newMemoryFactory)
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
        public virtual IMemoryBuffer<T> ChangeMemory(Memory<T> newMemory)
        {
            DisposeCore();

            Memory = newMemory;
            return this;
        }


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IMemoryBuffer{T}"/>。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        public virtual bool Equals(IMemoryBuffer<T> other)
        {
            other.NotNull(nameof(other));
            return Memory.ToArray().SequenceEqual(other.Memory.ToArray());
            //return Memory.Equals(other.Memory);
        }

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        public override bool Equals(object obj)
            => (obj is IMemoryBuffer<T> other) ? Equals(other) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 <see cref="int"/>。</returns>
        public override int GetHashCode()
            => Memory.GetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Memory.ToString();


        /// <summary>
        /// 释放核心对象。
        /// </summary>
        protected override void DisposeCore()
            => Memory.Pin().Dispose();


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractMemoryBuffer{T}"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractMemoryBuffer{T}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(AbstractMemoryBuffer<T> a, AbstractMemoryBuffer<T> b)
            => a.Equals(b);

        /// <summary>
        /// 是否不等。
        /// </summary>
        /// <param name="a">给定的 <see cref="AbstractMemoryBuffer{T}"/>。</param>
        /// <param name="b">给定的 <see cref="AbstractMemoryBuffer{T}"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(AbstractMemoryBuffer<T> a, AbstractMemoryBuffer<T> b)
            => !a.Equals(b);
    }
}
