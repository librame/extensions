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
    using KeyGenerators;

    static class KeyGeneratorEncryptionBuilderExtensions
    {
        internal static IEncryptionBuilder AddKeyGenerators(this IEncryptionBuilder builder)
        {
            builder.Services.TryAddScoped<IKeyGenerator, KeyGenerator>();

            return builder;
        }

    }
}
