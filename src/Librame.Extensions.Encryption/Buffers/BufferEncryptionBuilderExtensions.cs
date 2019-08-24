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
    static class BufferEncryptionBuilderExtensions
    {
        public static IEncryptionBuilder AddBuffers(this IEncryptionBuilder builder)
        {
            builder.Services.AddScoped(typeof(IEncryptionBuffer<,>), typeof(EncryptionBuffer<,>));

            return builder;
        }

    }
}
