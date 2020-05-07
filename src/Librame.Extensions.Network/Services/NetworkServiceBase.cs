#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using System.Text;

namespace Librame.Extensions.Network.Services
{
    using Core.Builders;
    using Core.Services;
    using Network.Builders;

    /// <summary>
    /// 网络服务基类。
    /// </summary>
    public class NetworkServiceBase : AbstractExtensionBuilderService<NetworkBuilderOptions>, INetworkService
    {
        /// <summary>
        /// 构造一个 <see cref="NetworkServiceBase"/>。
        /// </summary>
        /// <param name="serviceBase">给定的 <see cref="NetworkServiceBase"/>。</param>
        protected NetworkServiceBase(NetworkServiceBase serviceBase)
            : base(serviceBase?.Options, serviceBase?.LoggerFactory)
        {
            CoreOptions = serviceBase.CoreOptions;
        }

        /// <summary>
        /// 构造一个 <see cref="NetworkServiceBase"/>。
        /// </summary>
        /// <param name="dependency">给定的 <see cref="NetworkBuilderDependency"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected NetworkServiceBase(NetworkBuilderDependency dependency, ILoggerFactory loggerFactory)
            : base(dependency?.Options, loggerFactory)
        {
            CoreOptions = dependency.GetRequiredParentDependency<CoreBuilderDependency>().Options;
        }


        /// <summary>
        /// 核心构建器选项。
        /// </summary>
        /// <value>返回 <see cref="CoreBuilderOptions"/>。</value>
        public CoreBuilderOptions CoreOptions { get; }

        /// <summary>
        /// 字符编码。
        /// </summary>
        public Encoding Encoding
            => CoreOptions.Encoding;
    }
}
