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

    class KeyedHashService : ExtensionBuilderServiceBase<EncryptionBuilderOptions>, IKeyedHashService
    {
        public KeyedHashService(IKeyGenerator keyGenerator)
            : base(keyGenerator.CastTo<IKeyGenerator, ExtensionBuilderServiceBase<EncryptionBuilderOptions>>(nameof(keyGenerator)))
        {
            KeyGenerator = keyGenerator;
        }


        public IKeyGenerator KeyGenerator { get; }


        private IByteBuffer ComputeHash(IByteBuffer buffer, HMAC hmac)
        {
            return buffer.Change(memory =>
            {
                var hash = hmac.ComputeHash(memory.ToArray());
                Logger.LogDebug($"Compute HMAC hash: {hmac.HashName}");

                return hash;
            });
        }


        public IByteBuffer HmacMd5(IByteBuffer buffer, string identifier = null)
        {
            var key = KeyGenerator.GetHmacMd5Key(identifier);
            var hash = new HMACMD5(key);

            return ComputeHash(buffer, hash);
        }

        public IByteBuffer HmacSha1(IByteBuffer buffer, string identifier = null)
        {
            var key = KeyGenerator.GetHmacSha1Key(identifier);
            var hash = new HMACSHA1(key);

            return ComputeHash(buffer, hash);
        }

        public IByteBuffer HmacSha256(IByteBuffer buffer, string identifier = null)
        {
            var key = KeyGenerator.GetHmacSha256Key(identifier);
            var hash = new HMACSHA256(key);

            return ComputeHash(buffer, hash);
        }

        public IByteBuffer HmacSha384(IByteBuffer buffer, string identifier = null)
        {
            var key = KeyGenerator.GetHmacSha384Key(identifier);
            var hash = new HMACSHA384(key);

            return ComputeHash(buffer, hash);
        }

        public IByteBuffer HmacSha512(IByteBuffer buffer, string identifier = null)
        {
            var key = KeyGenerator.GetHmacSha512Key(identifier);
            var hash = new HMACSHA512(key);

            return ComputeHash(buffer, hash);
        }

    }
}
