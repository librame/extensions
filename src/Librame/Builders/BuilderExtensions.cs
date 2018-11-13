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
        /// 预配置构建器。
        /// </summary>
        /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{TBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder PreConfigureBuilder<TBuilderOptions>(this IBuilder builder,
            IConfiguration configuration = null, Action<TBuilderOptions> configureOptions = null)
             where TBuilderOptions : class, IBuilderOptions
        {
            if (configuration.IsNotDefault())
                builder.Services.Configure<TBuilderOptions>(configuration);

            if (configureOptions.IsNotDefault())
                builder.Services.Configure(configureOptions);

            return builder;
        }

    }
}
