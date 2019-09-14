﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 数据构建器静态扩展。
    /// </summary>
    public static class DataBuilderExtensions
    {
        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="builderAction">给定的选项配置动作。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IExtensionBuilder builder,
            Action<DataBuilderOptions> builderAction,
            Func<IExtensionBuilder, IDataBuilder> builderFactory = null)
        {
            builderAction.NotNull(nameof(builderAction));

            return builder.AddData(dependency =>
            {
                dependency.BuilderOptionsAction = builderAction;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加数据扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建数据构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDataBuilder"/>。</returns>
        public static IDataBuilder AddData(this IExtensionBuilder builder,
            Action<DataBuilderDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, IDataBuilder> builderFactory = null)
        {
            // Add Dependencies
            var dependency = dependencyAction.ConfigureDependencyOptions();

            // Add Builder
            builder.Services.OnlyConfigure(dependency.BuilderOptionsAction,
                dependency.BuilderOptionsName);

            var dataBuilder = builderFactory.NotNullOrDefault(()
                => b => new DataBuilder(b)).Invoke(builder);

            return dataBuilder
                .AddMediators()
                .AddServices()
                .AddStores();
        }

    }
}
