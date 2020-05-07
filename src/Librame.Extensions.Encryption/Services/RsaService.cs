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
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption.Services
{
    using Core.Services;
    using Encryption.Builders;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class RsaService : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IRsaService
    {
        public RsaService(ISigningCredentialsService signingCredentials)
            : base(signingCredentials.CastTo<ISigningCredentialsService, AbstractExtensionBuilderService<EncryptionBuilderOptions>>(nameof(signingCredentials)))
        {
            var credentials = signingCredentials.GetSigningCredentials(Options.SigningCredentialsKey);
            Source = credentials.ResolveRsa();

            SigningCredentials = signingCredentials;
        }


        public ISigningCredentialsService SigningCredentials { get; }

        public RSA Source { get; }


        public HashAlgorithmName SignHashAlgorithm { get; set; }
            = HashAlgorithmName.SHA256;

        public RSASignaturePadding SignaturePadding { get; set; }
            = RSASignaturePadding.Pkcs1;

        public RSAEncryptionPadding EncryptionPadding { get; set; }
            = RSAEncryptionPadding.Pkcs1;


        public byte[] SignData(byte[] buffer)
            => Source.SignData(buffer, SignHashAlgorithm, SignaturePadding);

        public byte[] SignHash(byte[] buffer)
            => Source.SignHash(buffer, SignHashAlgorithm, SignaturePadding);


        public bool VerifyData(byte[] buffer, byte[] signedBuffer)
            => Source.VerifyData(buffer, signedBuffer, SignHashAlgorithm, SignaturePadding);

        public bool VerifyHash(byte[] buffer, byte[] signedBuffer)
            => Source.VerifyHash(buffer, signedBuffer, SignHashAlgorithm, SignaturePadding);


        public byte[] Encrypt(byte[] buffer)
            => Source.Encrypt(buffer, EncryptionPadding);

        public byte[] Decrypt(byte[] buffer)
            => Source.Decrypt(buffer, EncryptionPadding);
    }
}
