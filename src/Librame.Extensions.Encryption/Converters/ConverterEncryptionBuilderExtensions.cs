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
    static class ConverterEncryptionBuilderExtensions
    {
        public static IEncryptionBuilder AddConverters(this IEncryptionBuilder builder)
        {
            builder.Services.AddScoped<ICiphertextConverter, CiphertextConverter>();
            builder.Services.AddScoped<IPlaintextConverter, PlaintextConverter>();

            return builder;
        }

    }
}
