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
        /// <param name="readOnlyBuffer">给定的 <see cref="IReadOnlyBuffer{T}"/>。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public static IBuffer<T> AsBuffer<T>(this IReadOnlyBuffer<T> readOnlyBuffer)
        {
            readOnlyBuffer.NotNull(nameof(readOnlyBuffer));

            return new Buffer<T>(readOnlyBuffer.Memory.ToArray());
        }

        /// <summary>
        /// 转换为只读缓冲区。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="buffer">给定的 <see cref="IBuffer{T}"/>。</param>
        /// <returns>返回 <see cref="IReadOnlyBuffer{T}"/>。</returns>
        public static IReadOnlyBuffer<T> AsReadOnlyBuffer<T>(this IBuffer<T> buffer)
        {
            return new ReadOnlyBuffer<T>(buffer.Memory);
        }

    }
}
