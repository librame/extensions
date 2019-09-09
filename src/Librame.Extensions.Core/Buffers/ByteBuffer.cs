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
    /// 字节缓冲区。
    /// </summary>
    public class ByteBuffer : Buffer<byte>, IByteBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="ByteBuffer"/>。
        /// </summary>
        /// <param name="memory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        public ByteBuffer(Memory<byte> memory)
            : base(memory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ByteBuffer"/>。
        /// </summary>
        /// <param name="array">给定的字节数组。</param>
        public ByteBuffer(byte[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public new IByteBuffer Copy()
            => new ByteBuffer(Memory);


        /// <summary>
        /// 隐式转换为字节存储器。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ByteBuffer"/>。</param>
        public static implicit operator Memory<byte>(ByteBuffer buffer)
            => buffer.Memory;

        /// <summary>
        /// 隐式转换为字节数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ByteBuffer"/>。</param>
        public static implicit operator byte[](ByteBuffer buffer)
            => buffer.Memory.ToArray();


        /// <summary>
        /// 显式转换为字节缓冲区。
        /// </summary>
        /// <param name="bytes">给定的 <see cref="Memory{Byte}"/>。</param>
        public static explicit operator ByteBuffer(Memory<byte> bytes)
            => new ByteBuffer(bytes);

        /// <summary>
        /// 显式转换为字节缓冲区。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        public static explicit operator ByteBuffer(byte[] bytes)
            => new ByteBuffer(bytes);

    }
}
