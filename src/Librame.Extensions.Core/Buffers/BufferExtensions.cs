#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions
{
    using Core;

    /// <summary>
    /// 缓冲区静态扩展。
    /// </summary>
    public static class BufferExtensions
    {
        /// <summary>
        /// 转换为缓冲区。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="buffer">给定的 <see cref="IReadOnlyBuffer{T}"/>。</param>
        /// <returns>返回 <see cref="Buffer{T}"/>。</returns>
        public static Buffer<T> AsBuffer<T>(this IReadOnlyBuffer<T> buffer)
            => new Buffer<T>(buffer.NotNull(nameof(buffer)).Memory.ToArray());

        /// <summary>
        /// 转换为只读缓冲区。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <returns>返回 <see cref="ReadOnlyBuffer{T}"/>。</returns>
        public static ReadOnlyBuffer<T> AsReadOnlyBuffer<T>(this IBuffer<T> buffer)
            => new ReadOnlyBuffer<T>(buffer.NotNull(nameof(buffer)).Memory);

    }
}
