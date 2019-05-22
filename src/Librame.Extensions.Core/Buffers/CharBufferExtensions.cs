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

namespace Librame.Extensions
{
    using Core;

    /// <summary>
    /// 字符缓冲区静态扩展。
    /// </summary>
    public static class CharBufferExtensions
    {
        /// <summary>
        /// 转换为字符缓冲区。
        /// </summary>
        /// <param name="chars">给定的字符数组。</param>
        /// <returns>返回 <see cref="ICharBuffer"/>。</returns>
        public static ICharBuffer AsCharBuffer(this char[] chars)
        {
            return new CharBuffer(chars);
        }

        /// <summary>
        /// 转换为字符只读缓冲区。
        /// </summary>
        /// <param name="chars">给定的字符数组。</param>
        /// <returns>返回 <see cref="IReadOnlyCharBuffer"/>。</returns>
        public static IReadOnlyCharBuffer AsReadOnlyCharBuffer(this char[] chars)
        {
            return new ReadOnlyCharBuffer(chars);
        }


        /// <summary>
        /// 转换为字符缓冲区。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str is null or empty.
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 <see cref="ICharBuffer"/>。</returns>
        public static ICharBuffer AsCharBuffer(this string str)
        {
            str.NotNullOrEmpty(nameof(str));

            return new CharBuffer(str.ToCharArray());
        }

        /// <summary>
        /// 转换为字符只读缓冲区。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// str is null or empty.
        /// </exception>
        /// <param name="str">给定的字符串。</param>
        /// <returns>返回 <see cref="IReadOnlyCharBuffer"/>。</returns>
        public static IReadOnlyCharBuffer AsReadOnlyCharBuffer(this string str)
        {
            str.NotNullOrEmpty(nameof(str));

            return new ReadOnlyCharBuffer(str.ToCharArray());
        }

    }
}
