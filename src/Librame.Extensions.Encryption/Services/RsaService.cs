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
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Core;

    class RsaService : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, IRsaService
    {
        private RSA _rsa = null;


        public RsaService(ISigningCredentialsService signingCredentials)
            : base(signingCredentials.CastTo<ISigningCredentialsService, ExtensionBuilderServiceBase<EncryptionBuilderOptions>>(nameof(signingCredentials)))
        {
            SigningCredentials = signingCredentials;

            InitializeRsa();
        }
        
        private void InitializeRsa()
        {
            if (_rsa.IsNull())
            {
                var credentials = SigningCredentials.GetSigningCredentials(Options.SigningCredentialsKey);
                _rsa = credentials.ResolveRsa();
            }
        }


        public ISigningCredentialsService SigningCredentials { get; }


        public HashAlgorithmName SignHashAlgorithm { get; set; }
            = HashAlgorithmName.SHA256;

        public RSASignaturePadding SignaturePadding { get; set; }
            = RSASignaturePadding.Pkcs1;

        public RSAEncryptionPadding EncryptionPadding { get; set; }
            = RSAEncryptionPadding.Pkcs1;


        public IByteBuffer SignData(IByteBuffer buffer)
            => buffer.Change(memory => _rsa.SignData(memory.ToArray(), SignHashAlgorithm, SignaturePadding));

        public IByteBuffer SignHash(IByteBuffer buffer)
            => buffer.Change(memory => _rsa.SignHash(memory.ToArray(), SignHashAlgorithm, SignaturePadding));


        public bool VerifyData(IByteBuffer buffer, IReadOnlyBuffer<byte> signedBuffer)
            => _rsa.VerifyData(buffer.Memory.ToArray(), signedBuffer.Memory.ToArray(), SignHashAlgorithm, SignaturePadding);

        public bool VerifyHash(IByteBuffer buffer, IReadOnlyBuffer<byte> signedBuffer)
            => _rsa.VerifyHash(buffer.Memory.ToArray(), signedBuffer.Memory.ToArray(), SignHashAlgorithm, SignaturePadding);


        public IByteBuffer Encrypt(IByteBuffer buffer)
            => buffer.Change(memory => _rsa.Encrypt(memory.ToArray(), EncryptionPadding));

        public IByteBuffer Decrypt(IByteBuffer buffer)
            => buffer.Change(memory => _rsa.Decrypt(memory.ToArray(), EncryptionPadding));

    }
}
