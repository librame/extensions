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
using System;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption.Services
{
    using Core.Services;
    using Encryption.Builders;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class HashService : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IHashService
    {
        [SuppressMessage("Microsoft.Cryptography", "CA5351:DoNotUseBrokenCryptographicAlgorithms")]
        private readonly Lazy<MD5> _md5
            = new Lazy<MD5>(() => MD5.Create());

        [SuppressMessage("Microsoft.Cryptography", "CA5350:DoNotUseWeakCryptographicAlgorithms")]
        private readonly Lazy<SHA1> _sha1
            = new Lazy<SHA1>(() => SHA1.Create());

        private readonly Lazy<SHA256> _sha256
            = new Lazy<SHA256>(() => SHA256.Create());

        private readonly Lazy<SHA384> _sha384
            = new Lazy<SHA384>(() => SHA384.Create());

        private readonly Lazy<SHA512> _sha512
            = new Lazy<SHA512>(() => SHA512.Create());


        public HashService(IRsaService rsa)
            : base(rsa.CastTo<IRsaService, AbstractExtensionBuilderService<EncryptionBuilderOptions>>(nameof(rsa)))
        {
            Rsa = rsa;
        }


        public IRsaService Rsa { get; }


        private byte[] ComputeHash(HashAlgorithm algorithm, byte[] buffer, bool isSigned = false)
        {
            buffer = algorithm.ComputeHash(buffer);
            Logger.LogDebug($"Compute hash: {algorithm.GetType().Name}");

            if (isSigned)
            {
                buffer = Rsa.SignHash(buffer);
                Logger.LogDebug($"Use rsa sign hash: {Rsa.SignHashAlgorithm.Name}");
            }

            return buffer;
        }

        
        public byte[] Md5(byte[] buffer, bool isSigned = false)
            => ComputeHash(_md5.Value, buffer, isSigned);

        public byte[] Sha1(byte[] buffer, bool isSigned = false)
            => ComputeHash(_sha1.Value, buffer, isSigned);

        public byte[] Sha256(byte[] buffer, bool isSigned = false)
            => ComputeHash(_sha256.Value, buffer, isSigned);

        public byte[] Sha384(byte[] buffer, bool isSigned = false)
            => ComputeHash(_sha384.Value, buffer, isSigned);

        public byte[] Sha512(byte[] buffer, bool isSigned = false)
            => ComputeHash(_sha512.Value, buffer, isSigned);
    }
}
