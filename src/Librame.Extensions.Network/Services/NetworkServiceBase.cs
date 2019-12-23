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
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Librame.Extensions.Network.Services
{
    using Builders;
    using Core.Builders;
    using Core.Services;

    /// <summary>
    /// 网络服务基类。
    /// </summary>
    public class NetworkServiceBase : AbstractExtensionBuilderService<NetworkBuilderOptions>, INetworkService
    {
        /// <summary>
        /// 构造一个 <see cref="NetworkServiceBase"/>。
        /// </summary>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected NetworkServiceBase(IOptions<CoreBuilderOptions> coreOptions,
            IOptions<NetworkBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            CoreOptions = coreOptions.NotNull(nameof(coreOptions)).Value;
            Encoding = CoreOptions.Encoding.Source;
        }

        /// <summary>
        /// 构造一个 <see cref="NetworkServiceBase"/>。
        /// </summary>
        /// <param name="serviceBase">给定的 <see cref="NetworkServiceBase"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "serviceBase")]
        protected NetworkServiceBase(NetworkServiceBase serviceBase)
            : base(serviceBase)
        {
            CoreOptions = serviceBase.CoreOptions;
            Encoding = CoreOptions.Encoding.Source;
        }


        /// <summary>
        /// 核心选项。
        /// </summary>
        /// <value>返回 <see cref="CoreBuilderOptions"/>。</value>
        public CoreBuilderOptions CoreOptions { get; }

        /// <summary>
        /// 字符编码。
        /// </summary>
        /// <value>返回 <see cref="System.Text.Encoding"/>。</value>
        public Encoding Encoding { get; }
    }
}
