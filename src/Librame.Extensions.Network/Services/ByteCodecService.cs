#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Network.Services
{
    using Builders;
    using Core.Builders;
    using Core.Services;
    using Encryption.Services;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ByteCodecService : NetworkServiceBase, IByteCodecService
    {
        private readonly ServiceFactory _serviceFactory;
        private readonly ISymmetricService _symmetric;


        public ByteCodecService(ServiceFactory serviceFactory)
            : base(serviceFactory?.GetService<IOptions<CoreBuilderOptions>>(),
                  serviceFactory?.GetService<IOptions<NetworkBuilderOptions>>(),
                  serviceFactory?.GetService<ILoggerFactory>())
        {
            _serviceFactory = serviceFactory;
            _symmetric = _serviceFactory.GetRequiredService<ISymmetricService>();
        }


        public string DecodeStringFromBytes(byte[] buffer, bool enableCodec)
        {
            buffer = Decode(buffer, enableCodec);

            return buffer.AsEncodingString(Encoding);
        }

        public string DecodeString(string encode, bool enableCodec)
        {
            var buffer = encode.FromBase64String();
            buffer = Decode(buffer, enableCodec);

            return buffer.AsEncodingString(Encoding);
        }

        public byte[] Decode(byte[] buffer, bool enableCodec)
        {
            if (enableCodec)
                return _symmetric.DecryptAes(buffer);

            return buffer;
        }


        public byte[] EncodeStringAsBytes(string str, bool enableCodec)
        {
            var buffer = str.FromEncodingString(Encoding);

            return Encode(buffer, enableCodec);
        }

        public string EncodeString(string str, bool enableCodec)
        {
            var buffer = str.FromEncodingString(Encoding);
            buffer = Encode(buffer, enableCodec);

            return buffer.AsBase64String();
        }

        public byte[] Encode(byte[] buffer, bool enableCodec)
        {
            if (enableCodec)
                return _symmetric.EncryptAes(buffer);

            return buffer;
        }

    }
}
