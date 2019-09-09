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
    /// 抽象字节缓冲器静态扩展。
    /// </summary>
    public static class AbstractionByteBufferExtensions
    {
        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <typeparam name="TByteBuffer">指定字节缓冲区类型。</typeparam>
        /// <param name="buffer">给定的字节缓冲区。</param>
        /// <param name="changeFactory">给定改变存储器的工厂方法。</param>
        /// <returns>返回字节缓冲区。</returns>
        public static TByteBuffer Change<TByteBuffer>(this TByteBuffer buffer, Func<Memory<byte>, Memory<byte>> changeFactory)
            where TByteBuffer : IByteBuffer
            => buffer.Change(changeFactory.Invoke(buffer.Memory));

        /// <summary>
        /// 改变存储器。
        /// </summary>
        /// <typeparam name="TByteBuffer">指定字节缓冲区类型。</typeparam>
        /// <param name="buffer">给定的字节缓冲区。</param>
        /// <param name="memory">给定的 <see cref="Memory{Byte}"/>。</param>
        /// <returns>返回字节缓冲区。</returns>
        public static TByteBuffer Change<TByteBuffer>(this TByteBuffer buffer, Memory<byte> memory)
            where TByteBuffer : IByteBuffer
        {
            buffer.Memory = memory;
            return buffer;
        }


        /// <summary>
        /// 清空存储器。
        /// </summary>
        /// <typeparam name="TByteBuffer">指定字节缓冲区类型。</typeparam>
        /// <param name="buffer">给定的字节缓冲区。</param>
        /// <returns>返回字节缓冲区。</returns>
        public static TByteBuffer Clear<TByteBuffer>(this TByteBuffer buffer)
            where TByteBuffer : IByteBuffer
        {
            buffer.Memory = Memory<byte>.Empty;
            return buffer;
        }

    }
}
