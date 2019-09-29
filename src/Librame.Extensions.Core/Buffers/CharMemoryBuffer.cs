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
    /// 字符型可读写的连续内存缓冲区。
    /// </summary>
    public class CharMemoryBuffer : MemoryBuffer<char>, ICharMemoryBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="CharMemoryBuffer"/>。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{Char}"/>。</param>
        public CharMemoryBuffer(Memory<char> memory)
            : base(memory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="CharMemoryBuffer"/>。
        /// </summary>
        /// <param name="array">给定的字符数组。</param>
        public CharMemoryBuffer(char[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 隐式转换为字符数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharMemoryBuffer"/>。</param>
        public static implicit operator char[](CharMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray();

        /// <summary>
        /// 隐式转换为字符内存缓冲区。
        /// </summary>
        /// <param name="array">给定的字符数组。</param>
        public static implicit operator CharMemoryBuffer(char[] array)
            => new CharMemoryBuffer(array);


        /// <summary>
        /// 显式转换为字符内存。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharMemoryBuffer"/>。</param>
        public static explicit operator Memory<char>(CharMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory;

        /// <summary>
        /// 显式转换为字符内存缓冲区。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{Byte}"/>。</param>
        public static explicit operator CharMemoryBuffer(Memory<char> memory)
            => new CharMemoryBuffer(memory);


        /// <summary>
        /// 显式转换为字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharMemoryBuffer"/>。</param>
        public static explicit operator string(CharMemoryBuffer buffer)
            => new string(buffer.NotNull(nameof(buffer)).Memory.ToArray());

        /// <summary>
        /// 显式转换为字符内存缓冲区。
        /// </summary>
        /// <param name="str">给定的字符数组。</param>
        public static explicit operator CharMemoryBuffer(string str)
            => new CharMemoryBuffer(str.NotEmpty(nameof(str)).ToCharArray());
    }
}
