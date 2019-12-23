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

namespace Librame.Extensions.Network.Builders
{
    using Services;

    static class ServiceNetworkBuilderExtensions
    {
        internal static INetworkBuilder AddServices(this INetworkBuilder builder)
        {
            builder.Services.TryAddScoped<IByteCodecService, ByteCodecService>();
            builder.Services.TryAddScoped<ICrawlerService, CrawlerService>();
            builder.Services.TryAddScoped<IEmailService, EmailService>();
            builder.Services.TryAddScoped<ISmsService, SmsService>();

            return builder;
        }

    }
}
