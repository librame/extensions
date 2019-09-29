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
    /// 字符型只读的连续内存缓冲区。
    /// </summary>
    public class CharReadOnlyMemoryBuffer : ReadOnlyMemoryBuffer<char>, ICharReadOnlyMemoryBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="CharReadOnlyMemoryBuffer"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的只读存储器。</param>
        public CharReadOnlyMemoryBuffer(ReadOnlyMemory<char> readOnlyMemory)
            : base(readOnlyMemory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="CharReadOnlyMemoryBuffer"/>。
        /// </summary>
        /// <param name="array">给定的字符数组。</param>
        public CharReadOnlyMemoryBuffer(char[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 隐式转换为字符数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharReadOnlyMemoryBuffer"/>。</param>
        public static implicit operator char[](CharReadOnlyMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray();

        /// <summary>
        /// 隐式转换为字符只读内存缓冲区。
        /// </summary>
        /// <param name="array">给定的字符数组。</param>
        public static implicit operator CharReadOnlyMemoryBuffer(char[] array)
            => new CharReadOnlyMemoryBuffer(array);


        /// <summary>
        /// 显式转换为字符内存。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharReadOnlyMemoryBuffer"/>。</param>
        public static explicit operator ReadOnlyMemory<char>(CharReadOnlyMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory;

        /// <summary>
        /// 显式转换为字符只读内存缓冲区。
        /// </summary>
        /// <param name="memory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        public static explicit operator CharReadOnlyMemoryBuffer(ReadOnlyMemory<char> memory)
            => new CharReadOnlyMemoryBuffer(memory);


        /// <summary>
        /// 显式转换为字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharReadOnlyMemoryBuffer"/>。</param>
        public static explicit operator string(CharReadOnlyMemoryBuffer buffer)
            => new string(buffer.NotNull(nameof(buffer)).Memory.ToArray());

        /// <summary>
        /// 显式转换为字符只读内存缓冲区。
        /// </summary>
        /// <param name="str">给定的字符数组。</param>
        public static explicit operator CharReadOnlyMemoryBuffer(string str)
            => new CharReadOnlyMemoryBuffer(str.NotEmpty(nameof(str)).ToCharArray());
    }
}
