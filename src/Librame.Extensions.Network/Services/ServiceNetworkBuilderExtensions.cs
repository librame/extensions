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

namespace Librame.Extensions.Network
{
    static class ServiceNetworkBuilderExtensions
    {
        public static INetworkBuilder AddServices(this INetworkBuilder builder)
        {
            builder.Services.AddScoped<IByteCodecService, ByteCodecService>();
            builder.Services.AddScoped<ICrawlerService, CrawlerService>();
            builder.Services.AddScoped<IEmailService, EmailService>();
            builder.Services.AddScoped<ISmsService, SmsService>();

            return builder;
        }

    }
}
