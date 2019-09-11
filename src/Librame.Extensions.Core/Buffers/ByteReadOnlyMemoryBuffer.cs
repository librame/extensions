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
        /// 隐式转换为 <see cref="ReadOnlyMemory{Byte}"/>。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ByteReadOnlyMemoryBuffer"/>。</param>
        public static implicit operator ReadOnlyMemory<byte>(ByteReadOnlyMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory;

        /// <summary>
        /// 隐式转换为字节数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ByteReadOnlyMemoryBuffer"/>。</param>
        public static implicit operator byte[](ByteReadOnlyMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray();


        /// <summary>
        /// 显式转换为 <see cref="ByteReadOnlyMemoryBuffer"/>。
        /// </summary>
        /// <param name="bytes">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        public static explicit operator ByteReadOnlyMemoryBuffer(ReadOnlyMemory<byte> bytes)
            => new ByteReadOnlyMemoryBuffer(bytes);

        /// <summary>
        /// 显式转换为 <see cref="ByteReadOnlyMemoryBuffer"/>。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        public static explicit operator ByteReadOnlyMemoryBuffer(byte[] bytes)
            => new ByteReadOnlyMemoryBuffer(bytes);
    }
}
