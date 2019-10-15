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
    static class ConverterEncryptionBuilderExtensions
    {
        internal static IEncryptionBuilder AddConverters(this IEncryptionBuilder builder)
        {
            builder.Services.TryAddScoped<ICiphertextConverter, CiphertextConverter>();
            builder.Services.TryAddScoped<IPlaintextConverter, PlaintextConverter>();

            return builder;
        }

    }
}
