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

namespace Librame.Extensions
{
    using Core;

    /// <summary>
    /// 字节缓冲区静态扩展。
    /// </summary>
    public static class ByteBufferExtensions
    {
        /// <summary>
        /// 还原字符编码字符串为字节缓冲区。
        /// </summary>
        /// <param name="str">给定的字符串。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回 <see cref="ByteBuffer"/>。</returns>
        public static ByteBuffer AsByteBufferFromEncodingString(this string str, Encoding encoding = null)
            => (ByteBuffer)str.FromEncodingString(encoding);

        /// <summary>
        /// 转换为字符编码字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <param name="encoding">给定的 <see cref="Encoding"/>（可选；默认使用 <see cref="Encoding.UTF8"/>）。</param>
        /// <returns>返回字符串。</returns>
        public static string AsEncodingString(this IByteBuffer buffer, Encoding encoding = null)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray().AsEncodingString(encoding);


        /// <summary>
        /// 还原 BASE64 字符串为字节缓冲区。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <returns>返回 <see cref="ByteBuffer"/>。</returns>
        public static ByteBuffer AsByteBufferFromBase64String(this string base64String)
            => (ByteBuffer)base64String.FromBase64String();

        /// <summary>
        /// 还原 16 进制字符串为字节缓冲区。
        /// </summary>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <returns>返回 <see cref="ByteBuffer"/>。</returns>
        public static ByteBuffer AsByteBufferFromHexString(this string hexString)
            => (ByteBuffer)hexString.FromHexString();


        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase64String(this IByteBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray().AsBase64String();

        /// <summary>
        /// 转换为 16 进制字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsHexString(this IByteBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray().AsHexString();

    }
}
