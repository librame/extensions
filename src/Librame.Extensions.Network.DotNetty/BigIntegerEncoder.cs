#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using DotNetty.Codecs;
using DotNetty.Transport.Channels;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Numerics;

namespace Librame.Extensions.Network.DotNetty
{
    /// <summary>
    /// 大整数编码器。
    /// </summary>
    public class BigIntegerEncoder : MessageToMessageEncoder<BigInteger>
    {
        /// <summary>
        /// 编码。
        /// </summary>
        /// <param name="context">给定的 <see cref="IChannelHandlerContext"/>。</param>
        /// <param name="message">给定的 <see cref="BigInteger"/>。</param>
        /// <param name="output">给定的输出对象列表。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected override void Encode(IChannelHandlerContext context, BigInteger message, List<object> output)
        {
            context.NotNull(nameof(context));
            output.NotNull(nameof(output));

            var buffer = context.Allocator.Buffer();
            
            //https://msdn.microsoft.com/en-us/library/system.numerics.biginteger.tobytearray(v=vs.110).aspx
            //BigInteger.ToByteArray() return a Little-Endian bytes
            //IByteBuffer is Big-Endian by default
            var data = message.ToByteArray();
            buffer.WriteByte((byte)'F');
            buffer.WriteInt(data.Length);
            buffer.WriteBytes(data);
            output.Add(buffer);
        }

    }
}