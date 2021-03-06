﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core.Services
{
    using Builders;

    /// <summary>
    /// 抽象扩展构建器服务。
    /// </summary>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractExtensionBuilderService<TBuilderOptions> : AbstractService
        where TBuilderOptions : class, IExtensionBuilderOptions, new()
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilderService{TBuilderOptions}"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{TBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractExtensionBuilderService(IOptions<TBuilderOptions> options, ILoggerFactory loggerFactory)
            : this(options?.Value, loggerFactory)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilderService{TBuilderOptions}"/>。
        /// </summary>
        /// <param name="options">给定的 <typeparamref name="TBuilderOptions"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractExtensionBuilderService(TBuilderOptions options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options.NotNull(nameof(options));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilderService{TBuilderOptions}"/>。
        /// </summary>
        /// <param name="builderService">给定的 <see cref="AbstractExtensionBuilderService{TBuilderOptions}"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        protected AbstractExtensionBuilderService(AbstractExtensionBuilderService<TBuilderOptions> builderService)
            : base(builderService)
        {
            Options = builderService.Options;
        }


        /// <summary>
        /// 构建器选项。
        /// </summary>
        /// <value>返回 <typeparamref name="TBuilderOptions"/>。</value>
        public TBuilderOptions Options { get; }
    }
}
