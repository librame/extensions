#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Options;

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 抽象请求程序。
    /// </summary>
    public abstract class AbstractRequester : AbstractBuilderOptionsEncoding, IRequestFactory
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractRequester"/> 实例。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        protected AbstractRequester(IOptions<NetworkBuilderOptions> options,
            IOptions<CoreBuilderOptions> coreOptions)
            : base(coreOptions)
        {
            Options = options.NotNull(nameof(options)).Value;
        }


        /// <summary>
        /// 网络构建器选项。
        /// </summary>
        public NetworkBuilderOptions Options { get; }
    }
}
