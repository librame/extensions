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
    using Options;

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
        /// <param name="builderOptionsType">给定的构建器选项类型。</param>
        /// <param name="converter">给定的结果构建器转换方法。</param>
        /// <param name="builderOptions">给定的结果构建器选项。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="postConfigureOptions">给定的 <see cref="Action{TResultBuilderOptions}"/>（可选）。</param>
        /// <returns>返回结果构建器。</returns>
        public static TResultBuilder AddBuilder<TBuilder, TResultBuilder, TResultBuilderOptions>(this TBuilder builder,
            Type builderOptionsType, Func<TBuilder, TResultBuilder> converter, TResultBuilderOptions builderOptions = null,
            IConfiguration configuration = null, Action<TResultBuilderOptions> postConfigureOptions = null)
            where TBuilder : class, IBuilder
            where TResultBuilder : class, IBuilder
            where TResultBuilderOptions : class, IBuilderOptions
        {
            builderOptions.NotDefault(nameof(builderOptions));

            if (configuration.IsNotDefault())
                configuration.Bind(builderOptions);

            builder.Services.ConfigureOptions(builderOptionsType, builderOptions);

            if (postConfigureOptions.IsNotDefault())
                builder.Services.PostConfigure(postConfigureOptions);

            return converter?.Invoke(builder);
        }

    }
}
