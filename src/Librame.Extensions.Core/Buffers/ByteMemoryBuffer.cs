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
    /// 字节型可读写的连续内存缓冲区。
    /// </summary>
    public class ByteMemoryBuffer : MemoryBuffer<byte>, IByteMemoryBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="ByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="memory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        public ByteMemoryBuffer(Memory<byte> memory)
            : base(memory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="array">给定的字节数组。</param>
        public ByteMemoryBuffer(byte[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 隐式转换为 <see cref="Memory{Byte}"/>。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ByteMemoryBuffer"/>。</param>
        public static implicit operator Memory<byte>(ByteMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory;

        /// <summary>
        /// 隐式转换为字节数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ByteMemoryBuffer"/>。</param>
        public static implicit operator byte[](ByteMemoryBuffer buffer)
            => buffer.NotNull(nameof(buffer)).Memory.ToArray();


        /// <summary>
        /// 显式转换为 <see cref="ByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="bytes">给定的 <see cref="Memory{Byte}"/>。</param>
        public static explicit operator ByteMemoryBuffer(Memory<byte> bytes)
            => new ByteMemoryBuffer(bytes);

        /// <summary>
        /// 显式转换为 <see cref="ByteMemoryBuffer"/>。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        public static explicit operator ByteMemoryBuffer(byte[] bytes)
            => new ByteMemoryBuffer(bytes);
    }
}
