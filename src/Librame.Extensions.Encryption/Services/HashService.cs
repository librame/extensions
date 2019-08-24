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
using System.Security.Cryptography;

namespace Librame.Extensions.Encryption
{
    using Core;

    class HashService : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, IHashService
    {
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
        {
            return ComputeHash(MD5.Create(), buffer, isSigned);
        }

        public IByteBuffer Sha1(IByteBuffer buffer, bool isSigned = false)
        {
            return ComputeHash(SHA1.Create(), buffer, isSigned);
        }

        public IByteBuffer Sha256(IByteBuffer buffer, bool isSigned = false)
        {
            return ComputeHash(SHA256.Create(), buffer, isSigned);
        }

        public IByteBuffer Sha384(IByteBuffer buffer, bool isSigned = false)
        {
            return ComputeHash(SHA384.Create(), buffer, isSigned);
        }

        public IByteBuffer Sha512(IByteBuffer buffer, bool isSigned = false)
        {
            return ComputeHash(SHA512.Create(), buffer, isSigned);
        }

    }
}
