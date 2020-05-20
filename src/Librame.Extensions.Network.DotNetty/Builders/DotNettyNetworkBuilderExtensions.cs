#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core.Builders;
using Librame.Extensions.Core.Options;
using Librame.Extensions.Core.Services;
using Librame.Extensions.Network.Builders;
using Librame.Extensions.Network.DotNetty;
using Librame.Extensions.Network.DotNetty.Demo;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// DotNetty 网络构建器静态扩展。
    /// </summary>
    public static class DotNettyNetworkBuilderExtensions
    {
        /// <summary>
        /// 添加 DotNetty 扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddDotNetty(this INetworkBuilder builder,
            Action<DotNettyDependency> configureDependency = null)
            => builder.AddDotNetty<DotNettyDependency>(configureDependency);

        /// <summary>
        /// 添加 DotNetty 扩展。
        /// </summary>
        /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
        /// <param name="builder">给定的 <see cref="INetworkBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static INetworkBuilder AddDotNetty<TDependency>(this INetworkBuilder builder,
            Action<TDependency> configureDependency = null)
            where TDependency : DotNettyDependency
        {
            // Clear Options Cache
            ConsistencyOptionsCache.TryRemove<DotNettyOptions>();

            AddDotNettyServiceCharacteristics();

            // Add Builder Dependency
            var dependency = builder.AddBuilderDependency(out var dependencyType, configureDependency);
            builder.Services.TryAddReferenceBuilderDependency<DotNettyDependency>(dependency, dependencyType);

            builder.SetProperty(p => p.DotNettyDependency, dependency);

            // Configure Builder
            return builder.AddDotNettyServices();
        }


        private static INetworkBuilder AddDotNettyServices(this INetworkBuilder builder)
        {
            builder.AddService<IBootstrapWrapperFactory, BootstrapWrapperFactory>();
            builder.AddService<IBootstrapWrapper, BootstrapWrapper>();
            builder.AddService<IServerBootstrapWrapper, ServerBootstrapWrapper>();
            builder.AddService(typeof(IBootstrapWrapper<,>), typeof(BootstrapWrapper<,>));

            // Demo
            builder.AddService<IDiscardClient, DiscardClient>();
            builder.AddService<IDiscardServer, DiscardServer>();
            builder.AddService<IEchoClient, EchoClient>();
            builder.AddService<IEchoServer, EchoServer>();
            builder.AddService<IFactorialClient, FactorialClient>();
            builder.AddService<IFactorialServer, FactorialServer>();
            builder.AddService<IHttpServer, HttpServer>();
            builder.AddService<IQuoteOfTheMomentClient, QuoteOfTheMomentClient>();
            builder.AddService<IQuoteOfTheMomentServer, QuoteOfTheMomentServer>();
            builder.AddService<ISecureChatClient, SecureChatClient>();
            builder.AddService<ISecureChatServer, SecureChatServer>();
            builder.AddService<ITelnetClient, TelnetClient>();
            builder.AddService<ITelnetServer, TelnetServer>();
            builder.AddService<IWebSocketClient, WebSocketClient>();
            builder.AddService<IWebSocketServer, WebSocketServer>();

            return builder;
        }

        private static void AddDotNettyServiceCharacteristics()
        {
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IBootstrapWrapperFactory>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IBootstrapWrapper>(ServiceCharacteristics.Transient());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IServerBootstrapWrapper>(ServiceCharacteristics.Transient());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd(typeof(IBootstrapWrapper<,>), ServiceCharacteristics.Transient());

            // Demo
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IDiscardClient>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IDiscardServer>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IEchoClient>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IEchoServer>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IFactorialClient>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IFactorialServer>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IHttpServer>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IQuoteOfTheMomentClient>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IQuoteOfTheMomentServer>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<ISecureChatClient>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<ISecureChatServer>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<ITelnetClient>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<ITelnetServer>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IWebSocketClient>(ServiceCharacteristics.Singleton());
            NetworkBuilderServiceCharacteristicsRegistration.Register
                .TryAdd<IWebSocketServer>(ServiceCharacteristics.Singleton());
        }

    }
}
