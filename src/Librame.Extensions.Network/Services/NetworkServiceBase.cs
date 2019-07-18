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
using Microsoft.Extensions.Options;
using System.Text;

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 网络服务基类。
    /// </summary>
    public class NetworkServiceBase : AbstractService, INetworkService
    {
        /// <summary>
        /// 构造一个 <see cref="NetworkServiceBase"/> 实例。
        /// </summary>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public NetworkServiceBase(IOptions<CoreBuilderOptions> coreOptions,
            IOptions<NetworkBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            CoreOptions = coreOptions.NotNull(nameof(coreOptions)).Value;
            Options = options.NotNull(nameof(options)).Value;
            Encoding = CoreOptions.Encoding;
        }


        /// <summary>
        /// 核心选项。
        /// </summary>
        public CoreBuilderOptions CoreOptions { get; }

        /// <summary>
        /// 网络构建器选项。
        /// </summary>
        /// <value>返回 <see cref="NetworkBuilderOptions"/>。</value>
        public NetworkBuilderOptions Options { get; }

        /// <summary>
        /// 字符编码。
        /// </summary>
        public Encoding Encoding { get; set; }
    }
}
