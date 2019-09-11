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
        /// 隐式转换为 <see cref="ReadOnlyMemory{Char}"/>。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharReadOnlyMemoryBuffer"/>。</param>
        public static implicit operator ReadOnlyMemory<char>(CharReadOnlyMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory;

        /// <summary>
        /// 隐式转换为字符数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharReadOnlyMemoryBuffer"/>。</param>
        public static implicit operator char[](CharReadOnlyMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray();

        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="CharReadOnlyMemoryBuffer"/>。</param>
        public static implicit operator string(CharReadOnlyMemoryBuffer buffer)
            => new string(buffer.NotNull(nameof(buffer)).Memory.ToArray());


        /// <summary>
        /// 显式转换为 <see cref="CharReadOnlyMemoryBuffer"/>。
        /// </summary>
        /// <param name="chars">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        public static explicit operator CharReadOnlyMemoryBuffer(ReadOnlyMemory<char> chars)
            => new CharReadOnlyMemoryBuffer(chars);

        /// <summary>
        /// 显式转换为 <see cref="CharReadOnlyMemoryBuffer"/>。
        /// </summary>
        /// <param name="chars">给定的字符数组。</param>
        public static explicit operator CharReadOnlyMemoryBuffer(char[] chars)
            => new CharReadOnlyMemoryBuffer(chars);

        /// <summary>
        /// 显式转换为 <see cref="CharReadOnlyMemoryBuffer"/>。
        /// </summary>
        /// <param name="str">给定的字符数组。</param>
        public static explicit operator CharReadOnlyMemoryBuffer(string str)
            => new CharReadOnlyMemoryBuffer(str.NotNullOrEmpty(nameof(str)).ToCharArray());
    }
}
