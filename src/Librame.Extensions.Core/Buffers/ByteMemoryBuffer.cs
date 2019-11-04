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
    }
}
