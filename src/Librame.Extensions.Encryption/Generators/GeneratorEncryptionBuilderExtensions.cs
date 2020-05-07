#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Librame.Extensions.Encryption.Builders
{
    using Generators;

    static class GeneratorEncryptionBuilderExtensions
    {
        internal static IEncryptionBuilder AddKeyGenerators(this IEncryptionBuilder builder)
        {
            builder.Services.TryAddSingleton<IKeyGenerator, KeyGenerator>();
            builder.Services.TryAddSingleton<IVectorGenerator, VectorGenerator>();

            return builder;
        }

    }
}
