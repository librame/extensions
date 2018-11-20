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
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.Extensions.Options;
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
        /// <param name="converter">给定的结果构建器转换方法。</param>
        /// <param name="builderOptionsType">给定的构建器选项类型。</param>
        /// <param name="builderOptions">给定的结果构建器选项。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="postConfigureOptions">给定的 <see cref="Action{TResultBuilderOptions}"/>（可选）。</param>
        /// <returns>返回结果构建器。</returns>
        public static TResultBuilder AddBuilder<TBuilder, TResultBuilder, TResultBuilderOptions>(this TBuilder builder,
            Func<TBuilder, TResultBuilder> converter, Type builderOptionsType, TResultBuilderOptions builderOptions = null,
            IConfiguration configuration = null, Action<TResultBuilderOptions> postConfigureOptions = null)
            where TBuilder : class, IBuilder
            where TResultBuilder : class, IBuilder
            where TResultBuilderOptions : class, IBuilderOptions
        {
            builderOptions.NotDefault(nameof(builderOptions));

            OptionsRegistration.AddOptions(builderOptionsType, builderOptions);

            if (configuration.IsNotDefault())
                configuration.Bind(builderOptions);

            if (postConfigureOptions.IsNotDefault())
                builder.Services.PostConfigure(postConfigureOptions);

            builder.ReplaceOptionsFactory();

            return converter?.Invoke(builder);
        }


        /// <summary>
        /// 替换选项工厂。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="optionsFactoryType">给定的选项工厂类型（可选；默认使用 <see cref="EnhancedOptionsFactory{TOptions}"/>）。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder ReplaceOptionsFactory(this IBuilder builder, Type optionsFactoryType = null)
        {
            var baseOptionsFactoryType = typeof(IOptionsFactory<>);

            if (optionsFactoryType.IsNotDefault())
                baseOptionsFactoryType.CanAssignableFromType(optionsFactoryType);

            builder.Services.Replace(ServiceDescriptor.Transient(baseOptionsFactoryType,
                optionsFactoryType.AsValueOrDefault(() => typeof(EnhancedOptionsFactory<>))));

            return builder;
        }

    }
}
