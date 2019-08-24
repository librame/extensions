#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Encryption
{
    static class ServiceEncryptionBuilderExtensions
    {
        public static IEncryptionBuilder AddServices(this IEncryptionBuilder builder)
        {
            builder.Services.AddScoped<IHashService, HashService>();
            builder.Services.AddScoped<IKeyedHashService, KeyedHashService>();
            builder.Services.AddScoped<IRsaService, RsaService>();
            builder.Services.AddScoped<ISymmetricService, SymmetricService>();

            return builder;
        }

    }
}
