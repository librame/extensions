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
    /// 字节缓冲区静态扩展。
    /// </summary>
    public static class ByteBufferExtensions
    {
        /// <summary>
        /// 转换为字节缓冲区。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer AsByteBuffer(this byte[] bytes)
        {
            return new ByteBuffer(bytes);
        }

        /// <summary>
        /// 转换为字节只读缓冲区。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        /// <returns>返回 <see cref="IReadOnlyByteBuffer"/>。</returns>
        public static IReadOnlyByteBuffer AsReadOnlyByteBuffer(this byte[] bytes)
        {
            return new ReadOnlyByteBuffer(bytes);
        }


        /// <summary>
        /// 还原 BASE64 字符串为字节缓冲区。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer AsByteBufferFromBase64String(this string base64String)
        {
            return new ByteBuffer(base64String.FromBase64String());
        }

        /// <summary>
        /// 还原 16 进制字符串为字节缓冲区。
        /// </summary>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public static IByteBuffer AsByteBufferFromHexString(this string hexString)
        {
            return new ByteBuffer(hexString.FromHexString());
        }


        /// <summary>
        /// 转换为 BASE64 字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsBase64String(this IByteBuffer buffer)
        {
            return buffer.Memory.ToArray().AsBase64String();
        }

        /// <summary>
        /// 转换为 16 进制字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="IByteBuffer"/>。</param>
        /// <returns>返回字符串。</returns>
        public static string AsHexString(this IByteBuffer buffer)
        {
            return buffer.Memory.ToArray().AsHexString();
        }

    }
}
