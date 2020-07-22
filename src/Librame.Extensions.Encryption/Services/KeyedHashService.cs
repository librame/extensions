#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Encryption.Services
{
    using Core.Services;
    using Core.Tokens;
    using Encryption.Builders;
    using Encryption.Generators;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class KeyedHashService : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IKeyedHashService
    {
        public KeyedHashService(IKeyGenerator keyGenerator)
            : base(keyGenerator.CastTo<IKeyGenerator, AbstractExtensionBuilderService<EncryptionBuilderOptions>>(nameof(keyGenerator)))
        {
            KeyGenerator = keyGenerator;
        }


        public IKeyGenerator KeyGenerator { get; }


        public byte[] HmacMd5(byte[] buffer, out byte[] key, SecurityToken token = null)
        {
            key = KeyGenerator.GetHmacMd5Key(token);
            return buffer.HmacMd5(key);
        }

        public byte[] HmacSha1(byte[] buffer, out byte[] key, SecurityToken token = null)
        {
            key = KeyGenerator.GetHmacSha1Key(token);
            return buffer.HmacSha1(key);
        }

        public byte[] HmacSha256(byte[] buffer, out byte[] key, SecurityToken token = null)
        {
            key = KeyGenerator.GetHmacSha256Key(token);
            return buffer.HmacSha256(key);
        }

        public byte[] HmacSha384(byte[] buffer, out byte[] key, SecurityToken token = null)
        {
            key = KeyGenerator.GetHmacSha384Key(token);
            return buffer.HmacSha384(key);
        }

        public byte[] HmacSha512(byte[] buffer, out byte[] key, SecurityToken token = null)
        {
            key = KeyGenerator.GetHmacSha512Key(token);
            return buffer.HmacSha512(key);
        }

    }
}
