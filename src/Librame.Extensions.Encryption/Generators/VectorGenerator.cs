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
using System.Linq;

namespace Librame.Extensions.Encryption.Generators
{
    using Core.Services;
    using Core.Tokens;
    using Encryption.Builders;
    using Encryption.Resources;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class VectorGenerator : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IVectorGenerator
    {
        private readonly SecurityToken _defaultToken;


        public VectorGenerator(IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _defaultToken = Options.Token;
        }


        public byte[] GenerateVector(byte[] key, int length, SecurityToken token = null)
        {
            if (token.IsNull())
                token = _defaultToken;

            if (token.IsNull())
                throw new ArgumentNullException(InternalResource.ArgumentNullExceptionTokenBothNull);

            var readOnlyMemory = token.ToReadOnlyMemory();
            Logger.LogDebug($"Use security token: {token}");

            var initialVector = readOnlyMemory.ToArray().Concat(key).ToArray();
            return GeneratorHelper.GenerateBytes(initialVector, length, Options.GenerateRandomVector);
        }

    }
}
