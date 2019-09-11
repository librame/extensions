#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Core;

    class RsaService : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, IRsaService
    {
        private readonly Lazy<RSA> _rsa;


        public RsaService(ISigningCredentialsService signingCredentials)
            : base(signingCredentials.CastTo<ISigningCredentialsService, ExtensionBuilderServiceBase<EncryptionBuilderOptions>>(nameof(signingCredentials)))
        {
            SigningCredentials = signingCredentials;

            _rsa = new Lazy<RSA>(() =>
            {
                var credentials = SigningCredentials.GetSigningCredentials(Options.SigningCredentialsKey);
                return credentials.ResolveRsa();
            });
        }


        public ISigningCredentialsService SigningCredentials { get; }


        public HashAlgorithmName SignHashAlgorithm { get; set; }
            = HashAlgorithmName.SHA256;

        public RSASignaturePadding SignaturePadding { get; set; }
            = RSASignaturePadding.Pkcs1;

        public RSAEncryptionPadding EncryptionPadding { get; set; }
            = RSAEncryptionPadding.Pkcs1;


        public IByteMemoryBuffer SignData(IByteMemoryBuffer buffer)
        {
            buffer.ChangeMemory(memory => _rsa.Value.SignData(memory.ToArray(), SignHashAlgorithm, SignaturePadding));
            return buffer;
        }

        public IByteMemoryBuffer SignHash(IByteMemoryBuffer buffer)
        {
            buffer.ChangeMemory(memory => _rsa.Value.SignHash(memory.ToArray(), SignHashAlgorithm, SignaturePadding));
            return buffer;
        }


        public bool VerifyData(IByteMemoryBuffer buffer, IReadOnlyMemoryBuffer<byte> signedBuffer)
            => _rsa.Value.VerifyData(buffer.Memory.ToArray(), signedBuffer.Memory.ToArray(), SignHashAlgorithm, SignaturePadding);

        public bool VerifyHash(IByteMemoryBuffer buffer, IReadOnlyMemoryBuffer<byte> signedBuffer)
            => _rsa.Value.VerifyHash(buffer.Memory.ToArray(), signedBuffer.Memory.ToArray(), SignHashAlgorithm, SignaturePadding);


        public IByteMemoryBuffer Encrypt(IByteMemoryBuffer buffer)
        {
            buffer.ChangeMemory(memory => _rsa.Value.Encrypt(memory.ToArray(), EncryptionPadding));
            return buffer;
        }

        public IByteMemoryBuffer Decrypt(IByteMemoryBuffer buffer)
        {
            buffer.ChangeMemory(memory => _rsa.Value.Decrypt(memory.ToArray(), EncryptionPadding));
            return buffer;
        }
    }
}
