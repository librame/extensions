#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace Librame.Extensions.Network
{
    using Core;

    class ByteCodecService : NetworkServiceBase, IByteCodecService
    {
        private readonly IServiceProvider _serviceProvider;


        public ByteCodecService(IServiceProvider serviceProvider)
            : base(serviceProvider?.GetService<IOptions<CoreBuilderOptions>>(),
                  serviceProvider?.GetService<IOptions<NetworkBuilderOptions>>(),
                  serviceProvider?.GetService<ILoggerFactory>())
        {
            _serviceProvider = serviceProvider;
        }


        public string DecodeStringFromBytes(byte[] buffer, bool enableCodec)
        {
            buffer = Decode(buffer, enableCodec);

            return buffer.FromEncodingBytes(Encoding);
        }

        public string DecodeString(string encode, bool enableCodec)
        {
            var buffer = encode.FromBase64String();
            buffer = Decode(buffer, enableCodec);

            return buffer.FromEncodingBytes(Encoding);
        }

        public byte[] Decode(byte[] buffer, bool enableCodec)
        {
            if (enableCodec)
                return Options.ByteCodec.DecodeFactory?.Invoke(_serviceProvider, buffer) ?? buffer;

            return buffer;
        }


        public byte[] EncodeStringAsBytes(string str, bool enableCodec)
        {
            var buffer = str.AsEncodingBytes(Encoding);

            return Encode(buffer, enableCodec);
        }

        public string EncodeString(string str, bool enableCodec)
        {
            var buffer = str.AsEncodingBytes(Encoding);
            buffer = Encode(buffer, enableCodec);

            return buffer.AsBase64String();
        }

        public byte[] Encode(byte[] buffer, bool enableCodec)
        {
            if (enableCodec)
                return Options.ByteCodec.EncodeFactory?.Invoke(_serviceProvider, buffer) ?? buffer;

            return buffer;
        }

    }
}
