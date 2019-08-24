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
    static class KeyGeneratorEncryptionBuilderExtensions
    {
        public static IEncryptionBuilder AddKeyGenerators(this IEncryptionBuilder builder)
        {
            builder.Services.AddScoped<IKeyGenerator, KeyGenerator>();

            return builder;
        }

    }
}
