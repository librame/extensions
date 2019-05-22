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
        /// 构造一个 <see cref="ReadOnlyCharBuffer"/> 实例。
        /// </summary>
        /// <param name="readOnlyMemory">给定的只读存储器。</param>
        public ReadOnlyByteBuffer(ReadOnlyMemory<byte> readOnlyMemory)
            : base(readOnlyMemory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ReadOnlyCharBuffer"/> 实例。
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
        public virtual new IReadOnlyByteBuffer Copy()
        {
            return new ReadOnlyByteBuffer(Memory);
        }

    }
}
