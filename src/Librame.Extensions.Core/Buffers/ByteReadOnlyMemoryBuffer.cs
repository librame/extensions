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
    }
}
