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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Network.Services
{
    using Encryption.Services;
    using Network.Builders;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ByteCodecService : NetworkServiceBase, IByteCodecService
    {
        public ByteCodecService(ISymmetricService symmetric,
            NetworkBuilderDependency dependency, ILoggerFactory loggerFactory)
            : base(dependency, loggerFactory)
        {
            Symmetric = symmetric.NotNull(nameof(symmetric));
        }


        public ISymmetricService Symmetric { get; }


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
                return Symmetric.DecryptAes(buffer);

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
                return Symmetric.EncryptAes(buffer);

            return buffer;
        }

    }
}
