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
    internal class InternalNetworkBuilder : AbstractExtensionBuilder, INetworkBuilder
    {
        /// <summary>
        /// 构造一个 <see cref="InternalNetworkBuilder"/> 实例。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        public InternalNetworkBuilder(IExtensionBuilder builder)
            : base(builder)
        {
            Services.AddSingleton<INetworkBuilder>(this);
        }

    }
}
