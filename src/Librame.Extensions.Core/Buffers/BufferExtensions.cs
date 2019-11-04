#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Text;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 缓冲区静态扩展。
    /// </summary>
    public static class BufferExtensions
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


        /// <summary>
        /// 将字符串还原为 <see cref="ByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回 <see cref="ByteMemoryBuffer"/>。</returns>
        public static ByteMemoryBuffer AsByteBufferFromEncodingString(this string str, Encoding encoding = null)
            => new ByteMemoryBuffer(str.FromEncodingString(encoding));

        /// <summary>
        /// 转换为字符编码字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteMemoryBuffer"/>。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingString(this IByteMemoryBuffer buffer, Encoding encoding = null)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray().AsEncodingString(encoding);


        /// <summary>
        /// 将 BASE64 字符串还原为 <see cref="ByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <returns>返回 <see cref="ByteMemoryBuffer"/>。</returns>
        public static ByteMemoryBuffer AsByteBufferFromBase64String(this string base64String)
            => new ByteMemoryBuffer(base64String.FromBase64String());

        /// <summary>
        /// 将 16 进制字符串还原为 <see cref="ByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <returns>返回 <see cref="ByteMemoryBuffer"/>。</returns>
        public static ByteMemoryBuffer AsByteBufferFromHexString(this string hexString)
            => new ByteMemoryBuffer(hexString.FromHexString());
    }
}
