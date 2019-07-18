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
using System;

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 网络构建器静态扩展。
    /// </summary>
    public static class NetworkBuilderExtensions
    {
        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="setupAction">给定的 <see cref="Action{NetworkBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IExtensionBuilder builder,
            Action<NetworkBuilderOptions> setupAction = null)
        {
            return builder.AddNetwork(b => new InternalNetworkBuilder(b), setupAction);
        }

        /// <summary>
        /// 添加网络扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="createFactory">给定创建网络构建器的工厂方法。</param>
        /// <param name="setupAction">给定的 <see cref="Action{NetworkBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="INetworkBuilder"/>。</returns>
        public static INetworkBuilder AddNetwork(this IExtensionBuilder builder,
            Func<IExtensionBuilder, INetworkBuilder> createFactory,
            Action<NetworkBuilderOptions> setupAction = null)
        {
            createFactory.NotNull(nameof(createFactory));

            builder.Services.OnlyConfigure(setupAction);

            var networkBuilder = createFactory.Invoke(builder);

            return networkBuilder
                .AddRequests()
                .AddServices();
        }

    }
}
