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

    /// <summary>
    /// 网络构建器。
    /// </summary>
    public class NetworkBuilder : AbstractExtensionBuilder, INetworkBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="NetworkBuilder"/>。
        /// </summary>
        /// <param name="parentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependency">给定的 <see cref="NetworkBuilderDependency"/>。</param>
        public NetworkBuilder(IExtensionBuilder parentBuilder, NetworkBuilderDependency dependency)
            : base(parentBuilder, dependency)
        {
            Services.AddSingleton<INetworkBuilder>(this);

            AddNetworkServices();
        }


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


        /// <summary>
        /// DotNetty 依赖。
        /// </summary>
        public IExtensionBuilderDependency DotNettyDependency { get; private set; }


        /// <summary>
        /// 获取服务特征。
        /// </summary>
        /// <param name="serviceType">给定的服务类型。</param>
        /// <returns>返回 <see cref="ServiceCharacteristics"/>。</returns>
        public override ServiceCharacteristics GetServiceCharacteristics(Type serviceType)
            => NetworkBuilderServiceCharacteristicsRegistration.Register.GetOrDefault(serviceType);

    }
}
