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
    using Core;

    /// <summary>
    /// 内部网络构建器。
    /// </summary>
    internal class InternalNetworkBuilder : AbstractBuilder<NetworkBuilderOptions>, INetworkBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalNetworkBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="options">给定的 <see cref="NetworkBuilderOptions"/>。</param>
        public InternalNetworkBuilder(IBuilder builder, NetworkBuilderOptions options)
            : base(builder, options)
        {
            Services.AddSingleton<INetworkBuilder>(this);
        }

    }
}
