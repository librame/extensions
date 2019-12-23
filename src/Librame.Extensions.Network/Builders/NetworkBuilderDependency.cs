#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.Builders
{
    using Core.Builders;

    /// <summary>
    /// 网络构建器依赖。
    /// </summary>
    public class NetworkBuilderDependency : AbstractExtensionBuilderDependency<NetworkBuilderOptions>
    {
        /// <summary>
        /// 构造一个 <see cref="NetworkBuilderDependency"/>。
        /// </summary>
        public NetworkBuilderDependency()
            : base(nameof(NetworkBuilderDependency))
        {
        }

    }
}
