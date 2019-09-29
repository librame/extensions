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
    /// 字节型只读的连续内存缓冲区。
    /// </summary>
    public class ByteReadOnlyMemoryBuffer : ReadOnlyMemoryBuffer<byte>, IByteReadOnlyMemoryBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="ByteReadOnlyMemoryBuffer"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的只读存储器。</param>
        public ByteReadOnlyMemoryBuffer(ReadOnlyMemory<byte> readOnlyMemory)
            : base(readOnlyMemory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ByteReadOnlyMemoryBuffer"/>。
        /// </summary>
        /// <param name="array">给定的字节数组。</param>
        public ByteReadOnlyMemoryBuffer(byte[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 隐式转换为字节数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ByteReadOnlyMemoryBuffer"/>。</param>
        public static implicit operator byte[](ByteReadOnlyMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray();

        /// <summary>
        /// 隐式转换为字节只读内存缓冲区。
        /// </summary>
        /// <param name="array">给定的字节数组。</param>
        public static implicit operator ByteReadOnlyMemoryBuffer(byte[] array)
            => new ByteReadOnlyMemoryBuffer(array);


        /// <summary>
        /// 显式转换为字节只读内存。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ByteReadOnlyMemoryBuffer"/>。</param>
        public static explicit operator ReadOnlyMemory<byte>(ByteReadOnlyMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory;

        /// <summary>
        /// 显式转换为字节只读内存缓冲区。
        /// </summary>
        /// <param name="memory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        public static explicit operator ByteReadOnlyMemoryBuffer(ReadOnlyMemory<byte> memory)
            => new ByteReadOnlyMemoryBuffer(memory);
    }
}
