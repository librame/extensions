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
using System.Numerics;

namespace Librame.Extensions.Network.DotNetty.Internal
{
    internal class BigIntegerEncoder : MessageToMessageEncoder<BigInteger>
    {
        protected override void Encode(IChannelHandlerContext context, BigInteger message, List<object> output)
        {
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