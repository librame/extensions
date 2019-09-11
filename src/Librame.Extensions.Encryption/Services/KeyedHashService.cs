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

    class KeyedHashService : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, IKeyedHashService
    {
        private readonly Lazy<HMACMD5> _hmacMd5;
        private readonly Lazy<HMACSHA1> _hmacSha1;
        private readonly Lazy<HMACSHA256> _hmacSha256;
        private readonly Lazy<HMACSHA384> _hmacSha384;
        private readonly Lazy<HMACSHA512> _hmacSha512;


        public KeyedHashService(IKeyGenerator keyGenerator)
            : base(keyGenerator.CastTo<IKeyGenerator, ExtensionBuilderServiceBase<EncryptionBuilderOptions>>(nameof(keyGenerator)))
        {
            KeyGenerator = keyGenerator;

            _hmacMd5 = new Lazy<HMACMD5>(() =>
            {
                var hmac = new HMACMD5();
                hmac.Key = KeyGenerator.GetHmacMd5Key();
                return hmac;
            });

            _hmacSha1 = new Lazy<HMACSHA1>(() =>
            {
                var hmac = new HMACSHA1();
                hmac.Key = KeyGenerator.GetHmacSha1Key();
                return hmac;
            });

            _hmacSha256 = new Lazy<HMACSHA256>(() =>
            {
                var hmac = new HMACSHA256();
                hmac.Key = KeyGenerator.GetHmacSha256Key();
                return hmac;
            });

            _hmacSha384 = new Lazy<HMACSHA384>(() =>
            {
                var hmac = new HMACSHA384();
                hmac.Key = KeyGenerator.GetHmacSha384Key();
                return hmac;
            });

            _hmacSha512 = new Lazy<HMACSHA512>(() =>
            {
                var hmac = new HMACSHA512();
                hmac.Key = KeyGenerator.GetHmacSha512Key();
                return hmac;
            });
        }


        public IKeyGenerator KeyGenerator { get; }


        private IByteMemoryBuffer ComputeHash(IByteMemoryBuffer buffer, HMAC hmac)
        {
            buffer.ChangeMemory(memory =>
            {
                var hash = hmac.ComputeHash(memory.ToArray());
                Logger.LogDebug($"Compute HMAC hash: {hmac.HashName}");

                return hash;
            });

            return buffer;
        }


        public IByteMemoryBuffer HmacMd5(IByteMemoryBuffer buffer, UniqueAlgorithmIdentifier identifier = null)
        {
            if (identifier.IsNotNull())
            {
                var key = KeyGenerator.GetHmacMd5Key(identifier);
                _hmacMd5.Value.Key = key;
            }

            return ComputeHash(buffer, _hmacMd5.Value);
        }

        public IByteMemoryBuffer HmacSha1(IByteMemoryBuffer buffer, UniqueAlgorithmIdentifier identifier = null)
        {
            if (identifier.IsNotNull())
            {
                var key = KeyGenerator.GetHmacSha1Key(identifier);
                _hmacSha1.Value.Key = key;
            }

            return ComputeHash(buffer, _hmacSha1.Value);
        }

        public IByteMemoryBuffer HmacSha256(IByteMemoryBuffer buffer, UniqueAlgorithmIdentifier identifier = null)
        {
            if (identifier.IsNotNull())
            {
                var key = KeyGenerator.GetHmacSha256Key(identifier);
                _hmacSha256.Value.Key = key;
            }

            return ComputeHash(buffer, _hmacSha256.Value);
        }

        public IByteMemoryBuffer HmacSha384(IByteMemoryBuffer buffer, UniqueAlgorithmIdentifier identifier = null)
        {
            if (identifier.IsNotNull())
            {
                var key = KeyGenerator.GetHmacSha384Key(identifier);
                _hmacSha384.Value.Key = key;
            }

            return ComputeHash(buffer, _hmacSha384.Value);
        }

        public IByteMemoryBuffer HmacSha512(IByteMemoryBuffer buffer, UniqueAlgorithmIdentifier identifier = null)
        {
            if (identifier.IsNotNull())
            {
                var key = KeyGenerator.GetHmacSha512Key(identifier);
                _hmacSha512.Value.Key = key;
            }

            return ComputeHash(buffer, _hmacSha512.Value);
        }

    }
}
