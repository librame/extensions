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
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Core;

    class HashService : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, IHashService
    {
        private static readonly Lazy<MD5> _md5
            = new Lazy<MD5>(() => MD5.Create());

        private static readonly Lazy<SHA1> _sha1
            = new Lazy<SHA1>(() => SHA1.Create());

        private static readonly Lazy<SHA256> _sha256
            = new Lazy<SHA256>(() => SHA256.Create());

        private static readonly Lazy<SHA384> _sha384
            = new Lazy<SHA384>(() => SHA384.Create());

        private static readonly Lazy<SHA512> _sha512
            = new Lazy<SHA512>(() => SHA512.Create());


        public HashService(IRsaService rsa)
            : base(rsa.CastTo<IRsaService, ExtensionBuilderServiceBase<EncryptionBuilderOptions>>(nameof(rsa)))
        {
            Rsa = rsa;
        }


        public IRsaService Rsa { get; }


        private IByteBuffer ComputeHash(HashAlgorithm algorithm, IByteBuffer buffer, bool isSigned = false)
        {
            buffer.Change(memory =>
            {
                var hash = algorithm.ComputeHash(memory.ToArray());
                Logger.LogDebug($"Compute hash: {algorithm.GetType().Name}");

                return hash;
            });

            if (isSigned)
            {
                Rsa.SignHash(buffer);
                Logger.LogDebug($"Use rsa sign hash: {Rsa.SignHashAlgorithm.Name}");
            }

            return buffer;
        }

        
        public IByteBuffer Md5(IByteBuffer buffer, bool isSigned = false)
            => ComputeHash(_md5.Value, buffer, isSigned);

        public IByteBuffer Sha1(IByteBuffer buffer, bool isSigned = false)
            => ComputeHash(_sha1.Value, buffer, isSigned);

        public IByteBuffer Sha256(IByteBuffer buffer, bool isSigned = false)
            => ComputeHash(_sha256.Value, buffer, isSigned);

        public IByteBuffer Sha384(IByteBuffer buffer, bool isSigned = false)
            => ComputeHash(_sha384.Value, buffer, isSigned);

        public IByteBuffer Sha512(IByteBuffer buffer, bool isSigned = false)
            => ComputeHash(_sha512.Value, buffer, isSigned);

    }
}
