#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Encryption.Generators
{
    using Core.Tokens;
    using Core.Services;
    using Encryption.Builders;
    using Encryption.Resources;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class KeyGenerator : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IKeyGenerator
    {
        private readonly SecurityToken _defaultToken;


        public KeyGenerator(IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _defaultToken = Options.Token;
        }


        public byte[] GenerateKey(int length, SecurityToken token = null)
        {
            if (token.IsNull())
                token = _defaultToken;

            if (token.IsNull())
                throw new ArgumentNullException(InternalResource.ArgumentNullExceptionTokenBothNull);

            var readOnlyMemory = token.ToReadOnlyMemory();
            Logger.LogDebug($"Use security token: {token}");

            return GeneratorHelper.GenerateBytes(readOnlyMemory.ToArray(), length, Options.GenerateRandomKey);
        }

    }
}
