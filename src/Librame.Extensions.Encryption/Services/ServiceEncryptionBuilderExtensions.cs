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

namespace Librame.Extensions.Encryption
{
    static class ServiceEncryptionBuilderExtensions
    {
        internal static IEncryptionBuilder AddServices(this IEncryptionBuilder builder)
        {
            builder.Services.TryAddScoped<IHashService, HashService>();
            builder.Services.TryAddScoped<IKeyedHashService, KeyedHashService>();
            builder.Services.TryAddScoped<IRsaService, RsaService>();
            builder.Services.TryAddScoped<ISymmetricService, SymmetricService>();

            return builder;
        }

    }
}
