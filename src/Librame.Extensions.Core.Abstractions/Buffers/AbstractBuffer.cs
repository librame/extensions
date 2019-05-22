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
    /// 抽象缓冲区。
    /// </summary>
    /// <typeparam name="T">指定的类型。</typeparam>
    public abstract class AbstractBuffer<T> : IBuffer<T>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractBuffer{T}"/> 实例。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{T}"/>。</param>
        public AbstractBuffer(Memory<T> memory)
        {
            Memory = memory;
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractBuffer{T}"/> 实例。
        /// </summary>
        /// <param name="array">给定的数组。</param>
        public AbstractBuffer(T[] array)
        {
            Memory = array;
        }


        /// <summary>
        /// 存储器。
        /// </summary>
        /// <value>返回 <see cref="Memory{T}"/>。</value>
        public Memory<T> Memory { get; set; }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public abstract IBuffer<T> Copy();


        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        public virtual bool Equals(IBuffer<T> other)
        {
            other.NotNull(nameof(other));

            return Memory.Equals(other.Memory);
        }

        /// <summary>
        /// 是否相等。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回 <see cref="bool"/>。</returns>
        public override bool Equals(object obj)
        {
            if (obj is IBuffer<T> other)
                return Equals(other);

            return false;
        }


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 <see cref="int"/>。</returns>
        public override int GetHashCode()
        {
            return Memory.GetHashCode();
        }


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
        {
            return Memory.ToString();
        }

    }
}
