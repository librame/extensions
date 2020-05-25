#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Network.Builders
{
    using Core.Builders;
    using Core.Services;
    using Network.Requesters;
    using Network.Services;

    internal class NetworkBuilder : AbstractExtensionBuilder, INetworkBuilder
    {
        public NetworkBuilder(IExtensionBuilder parentBuilder, NetworkBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<INetworkBuilder>(this);

            AddNetworkServices();
        }


        public IExtensionBuilderDependency DotNettyDependency { get; private set; }


        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => NetworkBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);


        private void AddNetworkServices()
        {
            // Requesters
            AddServices(typeof(IUriRequester),
                typeof(HttpClientRequester), typeof(HttpWebRequester));

            // Services
            AddService<IByteCodecService, ByteCodecService>();
            AddService<ICrawlerService, CrawlerService>();
            AddService<IEmailService, EmailService>();
            AddService<ISmsService, SmsService>();
        }

    }
}
