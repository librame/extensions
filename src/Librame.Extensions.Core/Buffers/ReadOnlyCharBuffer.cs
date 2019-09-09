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
    /// 只读字符缓冲区。
    /// </summary>
    public class ReadOnlyCharBuffer : ReadOnlyBuffer<char>, IReadOnlyCharBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="ReadOnlyCharBuffer"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的只读存储器。</param>
        public ReadOnlyCharBuffer(ReadOnlyMemory<char> readOnlyMemory)
            : base(readOnlyMemory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ReadOnlyCharBuffer"/>。
        /// </summary>
        /// <param name="array">给定的字符数组。</param>
        public ReadOnlyCharBuffer(char[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyCharBuffer"/>。</returns>
        public new IReadOnlyCharBuffer Copy()
            => new ReadOnlyCharBuffer(Memory);


        /// <summary>
        /// 隐式转换为只读字符存储器。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyCharBuffer"/>。</param>
        public static implicit operator ReadOnlyMemory<char>(ReadOnlyCharBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory;

        /// <summary>
        /// 隐式转换为字符数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyCharBuffer"/>。</param>
        public static implicit operator char[](ReadOnlyCharBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray();

        /// <summary>
        /// 隐式转换为字符数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyCharBuffer"/>。</param>
        public static implicit operator string(ReadOnlyCharBuffer buffer)
            => new string(buffer.NotNull(nameof(buffer)).Memory.ToArray());


        /// <summary>
        /// 显式转换为只读字符缓冲区。
        /// </summary>
        /// <param name="chars">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        public static explicit operator ReadOnlyCharBuffer(ReadOnlyMemory<char> chars)
            => new ReadOnlyCharBuffer(chars);

        /// <summary>
        /// 显式转换为只读字符缓冲区。
        /// </summary>
        /// <param name="chars">给定的字符数组。</param>
        public static explicit operator ReadOnlyCharBuffer(char[] chars)
            => new ReadOnlyCharBuffer(chars);

        /// <summary>
        /// 显式转换为只读字符缓冲区。
        /// </summary>
        /// <param name="str">给定的字符数组。</param>
        public static explicit operator ReadOnlyCharBuffer(string str)
            => new ReadOnlyCharBuffer(str.NotNullOrEmpty(nameof(str)).ToCharArray());

    }
}
