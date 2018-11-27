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

namespace Librame.Buffers
{
    /// <summary>
    /// 字节缓冲区。
    /// </summary>
    public class ByteBuffer : Buffer<byte>, IByteBuffer
    {
        /// <summary>
        /// 构造一个 <see cref="ByteBuffer"/> 实例。
        /// </summary>
        /// <param name="readOnlyMemory">给定的 <see cref="ReadOnlyMemory{Byte}"/>。</param>
        public ByteBuffer(ReadOnlyMemory<byte> readOnlyMemory)
            : base(readOnlyMemory)
        {
            Memory = readOnlyMemory.ToArray();
        }

        /// <summary>
        /// 构造一个 <see cref="ByteBuffer"/> 实例。
        /// </summary>
        /// <param name="memory">给定的 <see cref="Memory{Byte}"/>。</param>
        public ByteBuffer(Memory<byte> memory)
            : base(memory)
        {
            Memory = memory;
        }


        /// <summary>
        /// 创建副本。
        /// </summary>
        /// <returns>返回 <see cref="IByteBuffer"/>。</returns>
        public virtual new IByteBuffer Copy()
        {
            return new ByteBuffer(Memory);
        }

        /// <summary>
        /// 创建只读副本。
        /// </summary>
        /// <returns>返回 <see cref="IReadOnlyByteBuffer"/>。</returns>
        public virtual new IReadOnlyByteBuffer CopyReadOnly()
        {
            return new ReadOnlyByteBuffer(ReadOnlyMemory);
        }

    }
}
