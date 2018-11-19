#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System;

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
            return new DefaultBuffer<T>().ChangeMemory(elements);
        }

        /// <summary>
        /// 转换为只读缓冲区。
        /// </summary>
        /// <param name="elements">给定的元素集合。</param>
        /// <returns>返回 <see cref="IReadOnlyBuffer{T}"/>。</returns>
        public static IReadOnlyBuffer<T> AsReadOnlyBuffer<T>(this T[] elements)
        {
            return new DefaultReadOnlyBuffer<T>().ChangeReadOnlyMemory(elements);
        }


        /// <summary>
        /// 转换为字符串缓冲区。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str is null or white space.
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>（可选；默认新建实例）。</param>
        /// <returns>返回 <see cref="IStringBuffer"/>。</returns>
        public static IStringBuffer AsStringBuffer(this string str, ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory.IsDefault())
                loggerFactory = new LoggerFactory();

            return new DefaultStringBuffer(str, loggerFactory.CreateLogger<DefaultStringBuffer>());
        }

        /// <summary>
        /// 转换为字符串只读缓冲区。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str is null or white space.
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>（可选；默认新建实例）。</param>
        /// <returns>返回 <see cref="IStringReadOnlyBuffer"/>。</returns>
        public static IStringReadOnlyBuffer AsStringReadOnlyBuffer(this string str, ILoggerFactory loggerFactory = null)
        {
            if (loggerFactory.IsDefault())
                loggerFactory = new LoggerFactory();

            return new DefaultStringReadOnlyBuffer(str, loggerFactory.CreateLogger<DefaultStringReadOnlyBuffer>());
        }


        #region AsString

        /// <summary>
        /// 还原 BASE64 字符串。
        /// </summary>
        /// <param name="base64String">给定的 BASE64 字符串。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public static IBuffer<byte> FromBase64StringAsBuffer(this string base64String)
        {
            return base64String.FromBase64String().AsBuffer();
        }

        /// <summary>
        /// 还原 16 进制字符串。
        /// </summary>
        /// <param name="hexString">给定的 16 进制字符串。</param>
        /// <returns>返回 <see cref="IBuffer{T}"/>。</returns>
        public static IBuffer<byte> FromHexStringAsBuffer(this string hexString)
        {
            return hexString.FromHexString().AsBuffer();
        }

        #endregion

    }
}
