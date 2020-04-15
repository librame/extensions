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
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption.Services
{
    using Core.Services;
    using Encryption.Builders;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class RsaService : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IRsaService
    {
        private readonly Lazy<RSA> _rsa;


        public RsaService(ISigningCredentialsService signingCredentials)
            : base(signingCredentials.CastTo<ISigningCredentialsService, AbstractExtensionBuilderService<EncryptionBuilderOptions>>(nameof(signingCredentials)))
        {
            SigningCredentials = signingCredentials;

            _rsa = new Lazy<RSA>(() =>
            {
                var credentials = SigningCredentials.GetSigningCredentials(Options.SigningCredentialsKey);
                return credentials.ResolveRsa();
            });
        }


        public ISigningCredentialsService SigningCredentials { get; }

        public RSA Source
            => _rsa.Value;


        public HashAlgorithmName SignHashAlgorithm { get; set; }
            = HashAlgorithmName.SHA256;

        public RSASignaturePadding SignaturePadding { get; set; }
            = RSASignaturePadding.Pkcs1;

        public RSAEncryptionPadding EncryptionPadding { get; set; }
            = RSAEncryptionPadding.Pkcs1;


        public byte[] SignData(byte[] buffer)
            => _rsa.Value.SignData(buffer, SignHashAlgorithm, SignaturePadding);

        public byte[] SignHash(byte[] buffer)
            => _rsa.Value.SignHash(buffer, SignHashAlgorithm, SignaturePadding);


        public bool VerifyData(byte[] buffer, byte[] signedBuffer)
            => _rsa.Value.VerifyData(buffer, signedBuffer, SignHashAlgorithm, SignaturePadding);

        public bool VerifyHash(byte[] buffer, byte[] signedBuffer)
            => _rsa.Value.VerifyHash(buffer, signedBuffer, SignHashAlgorithm, SignaturePadding);


        public byte[] Encrypt(byte[] buffer)
            => _rsa.Value.Encrypt(buffer, EncryptionPadding);

        public byte[] Decrypt(byte[] buffer)
            => _rsa.Value.Decrypt(buffer, EncryptionPadding);
    }
}
