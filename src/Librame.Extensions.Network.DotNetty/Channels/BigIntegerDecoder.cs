#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Buffers;
using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System;
using System.Collections.Generic;
using System.Numerics;

namespace Librame.Extensions.Network.DotNetty
{
    /// <summary>
    /// 大整数解码器。
    /// </summary>
    public class BigIntegerDecoder : ByteToMessageDecoder
    {
        /// <summary>
        /// 解码。
        /// </summary>
        /// <param name="context">给定的 <see cref="IChannelHandlerContext"/>。</param>
        /// <param name="input">给定的 <see cref="IByteBuffer"/>。</param>
        /// <param name="output">给定的输出对象列表。</param>
        protected override void Decode(IChannelHandlerContext context, IByteBuffer input, List<object> output)
        {
            if (input.ReadableBytes < 5)
            {
                return;
            }
            input.MarkReaderIndex();

            int magicNumber = input.ReadByte();
            if (magicNumber != 'F')
            {
                input.ResetReaderIndex();
                throw new Exception("Invalid magic number: " + magicNumber);
            }
            int dataLength = input.ReadInt();
            if (input.ReadableBytes < dataLength)
            {
                input.ResetReaderIndex();
                return;
            }
            var decoded = new byte[dataLength];
            input.ReadBytes(decoded);

            output.Add(new BigInteger(decoded));
        }
    }
}