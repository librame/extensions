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
        public virtual new IByteBuffer Copy()
        {
            return new ByteBuffer(Memory);
        }

    }
}
