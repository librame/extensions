#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Builders
{
    using Extensions;

    /// <summary>
    /// 构建器静态扩展。
    /// </summary>
    public static class BuilderExtensions
    {

        /// <summary>
        /// 添加构建器。
        /// </summary>
        /// <typeparam name="TBuilder">指定的构建器类型。</typeparam>
        /// <typeparam name="TResultBuilder">指定的结果构建器类型。</typeparam>
        /// <typeparam name="TResultBuilderOptions">指定的结果构建器选项类型。</typeparam>
        /// <param name="builder">给定的构建器。</param>
        /// <param name="configureOptions">给定的结果构建器选项（可选）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="converter">给定的结果构建器转换方法（可选）。</param>
        /// <returns>返回结果构建器。</returns>
        public static TResultBuilder AddBuilder<TBuilder, TResultBuilder, TResultBuilderOptions>(this TBuilder builder,
            Action<TResultBuilderOptions> configureOptions = null, IConfiguration configuration = null,
            Func<TBuilder, TResultBuilder> converter = null)
            where TBuilder : class, IBuilder
            where TResultBuilder : class, IBuilder
            where TResultBuilderOptions : class, IBuilderOptions
        {
            // 配置对象优先级较低
            if (configuration.IsNotDefault())
                builder.Services.Configure<TResultBuilderOptions>(configuration);

            // 配置动作优先级较高
            if (configureOptions.IsNotDefault())
                builder.Services.Configure(configureOptions);

            return converter?.Invoke(builder);
        }

    }
}
