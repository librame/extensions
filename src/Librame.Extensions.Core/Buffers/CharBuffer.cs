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
    /// 字符缓冲区。
    /// </summary>
    public class CharBuffer : Buffer<char>, ICharBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="CharBuffer"/>。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{Char}"/>。</param>
        public CharBuffer(Memory<char> memory)
            : base(memory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="CharBuffer"/>。
        /// </summary>
        /// <param name="array">给定的字符数组。</param>
        public CharBuffer(char[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="ICharBuffer"/>。</returns>
        public new ICharBuffer Copy()
            => new CharBuffer(Memory);


        /// <summary>
        /// 隐式转换为字符存储器。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharBuffer"/>。</param>
        public static implicit operator Memory<char>(CharBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory;

        /// <summary>
        /// 隐式转换为字符数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharBuffer"/>。</param>
        public static implicit operator char[](CharBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray();

        /// <summary>
        /// 隐式转换为字符数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharBuffer"/>。</param>
        public static implicit operator string(CharBuffer buffer)
            => new string(buffer.NotNull(nameof(buffer)).Memory.ToArray());


        /// <summary>
        /// 显式转换为字符缓冲区。
        /// </summary>
        /// <param name="chars">给定的 <see cref="Memory{Byte}"/>。</param>
        public static explicit operator CharBuffer(Memory<char> chars)
            => new CharBuffer(chars);

        /// <summary>
        /// 显式转换为字符缓冲区。
        /// </summary>
        /// <param name="chars">给定的字符数组。</param>
        public static explicit operator CharBuffer(char[] chars)
            => new CharBuffer(chars);

        /// <summary>
        /// 显式转换为只读字符缓冲区。
        /// </summary>
        /// <param name="str">给定的字符数组。</param>
        public static explicit operator CharBuffer(string str)
            => new CharBuffer(str.NotNullOrEmpty(nameof(str)).ToCharArray());

    }
}
