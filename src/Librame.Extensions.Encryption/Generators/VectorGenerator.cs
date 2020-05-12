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
using Microsoft.Extensions.Options;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Encryption.Generators
{
    using Core.Identifiers;
    using Core.Services;
    using Encryption.Builders;
    using Encryption.Resources;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class VectorGenerator : AbstractExtensionBuilderService<EncryptionBuilderOptions>, IVectorGenerator
    {
        private readonly SecurityIdentifier _defaultIdentifier;


        public VectorGenerator(IOptions<EncryptionBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            _defaultIdentifier = Options.Identifier;
        }


        public byte[] GenerateVector(byte[] key, int length, SecurityIdentifier identifier = null)
        {
            if (identifier.IsNull())
                identifier = _defaultIdentifier;

            if (identifier.IsNull())
                throw new ArgumentNullException(InternalResource.ArgumentNullExceptionIdentifierBothNull);

            var readOnlyMemory = identifier.ToReadOnlyMemory();
            Logger.LogDebug($"Use security identifier: {identifier}");

            var initialVector = readOnlyMemory.ToArray().Concat(key).ToArray();
            return GeneratorHelper.GenerateBytes(initialVector, length, Options.GenerateRandomVector);
        }

    }
}
