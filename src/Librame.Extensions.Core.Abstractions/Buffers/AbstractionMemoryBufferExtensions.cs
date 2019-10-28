#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象可读写的连续内存缓冲区静态扩展。
    /// </summary>
    public static class AbstractionMemoryBufferExtensions
    {
        /// <summary>
        /// 转换为 <see cref="MemoryBuffer{T}"/>。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="buffer">给定的 <see cref="IReadOnlyMemoryBuffer{T}"/>。</param>
        /// <returns>返回 <see cref="MemoryBuffer{T}"/>。</returns>
        public static MemoryBuffer<T> AsMemoryBuffer<T>(this IReadOnlyMemoryBuffer<T> buffer)
            => new MemoryBuffer<T>(buffer.NotNull(nameof(buffer)).Memory.ToArray());

        /// <summary>
        /// 转换为 <see cref="ReadOnlyMemoryBuffer{T}"/>。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="buffer">给定的 <see cref="IMemoryBuffer{T}"/>。</param>
        /// <returns>返回 <see cref="ReadOnlyMemoryBuffer{T}"/>。</returns>
        public static ReadOnlyMemoryBuffer<T> AsReadOnlyMemoryBuffer<T>(this IMemoryBuffer<T> buffer)
            => new ReadOnlyMemoryBuffer<T>(buffer.NotNull(nameof(buffer)).Memory);
    }
}
