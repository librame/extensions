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
    using Services;

    static class ServiceEncryptionBuilderExtensions
    {
        internal static IEncryptionBuilder AddServices(this IEncryptionBuilder builder)
        {
            builder.Services.TryAddSingleton<IHashService, HashService>();
            builder.Services.TryAddSingleton<IKeyedHashService, KeyedHashService>();
            builder.Services.TryAddSingleton<IRsaService, RsaService>();
            builder.Services.TryAddSingleton<ISymmetricService, SymmetricService>();

            return builder;
        }

    }
}
