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
    /// 只读字节缓冲区。
    /// </summary>
    public class ReadOnlyByteBuffer : ReadOnlyBuffer<byte>, IReadOnlyByteBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="ReadOnlyByteBuffer"/>。
        /// </summary>
        /// <param name="readOnlyMemory">给定的只读存储器。</param>
        public ReadOnlyByteBuffer(ReadOnlyMemory<byte> readOnlyMemory)
            : base(readOnlyMemory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ReadOnlyByteBuffer"/>。
        /// </summary>
        /// <param name="array">给定的字节数组。</param>
        public ReadOnlyByteBuffer(byte[] array)
            : base(array)
        {
        }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyByteBuffer"/>。</returns>
        public new IReadOnlyByteBuffer Copy()
            => new ReadOnlyByteBuffer(Memory);


        /// <summary>
        /// 隐式转换为只读字节存储器。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyByteBuffer"/>。</param>
        public static implicit operator ReadOnlyMemory<byte>(ReadOnlyByteBuffer buffer)
            => buffer.Memory;

        /// <summary>
        /// 隐式转换为字节数组。
        /// </summary>
        /// <param name="buffer">给定的 <see cref="ReadOnlyByteBuffer"/>。</param>
        public static implicit operator byte[](ReadOnlyByteBuffer buffer)
            => buffer.Memory.ToArray();

        /// <summary>
        /// 显式转换为只读字节缓冲区。
        /// </summary>
        /// <param name="bytes">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        public static explicit operator ReadOnlyByteBuffer(ReadOnlyMemory<byte> bytes)
            => new ReadOnlyByteBuffer(bytes);

        /// <summary>
        /// 显式转换为只读字节缓冲区。
        /// </summary>
        /// <param name="bytes">给定的字节数组。</param>
        public static explicit operator ReadOnlyByteBuffer(byte[] bytes)
            => new ReadOnlyByteBuffer(bytes);

    }
}
