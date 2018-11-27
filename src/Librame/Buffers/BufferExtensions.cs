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
    using Buffers;

    /// <summary>
    /// 缓冲区静态扩展。
    /// </summary>
    public static class BufferExtensions
    {
        
        /// <summary>
        /// 转换为缓冲区。
        /// </summary>
        /// <param name="elements">给定的元素集合。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public static IBuffer<T> AsBuffer<T>(this T[] elements)
        {
            return new Buffer<T>(elements);
        }

        /// <summary>
        /// 转换为只读缓冲区。
        /// </summary>
        /// <param name="elements">给定的元素集合。</param>
        /// <returns>返回 <see cref="IReadOnlyBuffer{T}"/>。</returns>
        public static IReadOnlyBuffer<T> AsReadOnlyBuffer<T>(this T[] elements)
        {
            return new ReadOnlyBuffer<T>(elements);
        }

    }
}
