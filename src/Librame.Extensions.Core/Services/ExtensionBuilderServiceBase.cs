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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 扩展构建器服务基类。
    /// </summary>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public class ExtensionBuilderServiceBase<TBuilderOptions> : AbstractService
        where TBuilderOptions: class, IExtensionBuilderOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="ExtensionBuilderServiceBase{TBuilderOptions}"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{TBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected ExtensionBuilderServiceBase(IOptions<TBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options.NotNull(nameof(options)).Value;
        }

        /// <summary>
        /// 构造一个 <see cref="ExtensionBuilderServiceBase{TBuilderOptions}"/>。
        /// </summary>
        /// <param name="serviceBase">给定的 <see cref="ExtensionBuilderServiceBase{TBuilderOptions}"/>。</param>
        protected ExtensionBuilderServiceBase(ExtensionBuilderServiceBase<TBuilderOptions> serviceBase)
            : base(serviceBase?.LoggerFactory)
        {
            Options = serviceBase.Options;
        }


        /// <summary>
        /// 构建器选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TBuilderOptions"/>。</value>
        public TBuilderOptions Options { get; }
    }
}
