#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core.Builders;
using Librame.Extensions.Storage.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 存储构建器静态扩展。
    /// </summary>
    public static class StorageBuilderExtensions
    {
        /// <summary>
        /// 添加存储扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureOptions">给定的选项配置动作。</param>
        /// <param name="builderFactory">给定创建存储构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IExtensionBuilder baseBuilder,
            Action<StorageBuilderOptions> configureOptions,
            Func<IExtensionBuilder, StorageBuilderDependency, IStorageBuilder> builderFactory = null)
        {
            configureOptions.NotNull(nameof(configureOptions));

            return baseBuilder.AddStorage(dependency =>
            {
                dependency.Builder.ConfigureOptions = configureOptions;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加存储扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建存储构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IExtensionBuilder baseBuilder,
            Action<StorageBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, StorageBuilderDependency, IStorageBuilder> builderFactory = null)
            => baseBuilder.AddStorage<StorageBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加存储扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建存储构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "baseBuilder")]
        public static IStorageBuilder AddStorage<TDependencyOptions>(this IExtensionBuilder baseBuilder,
            Action<TDependencyOptions> configureDependency = null,
            Func<IExtensionBuilder, TDependencyOptions, IStorageBuilder> builderFactory = null)
            where TDependencyOptions : StorageBuilderDependency, new()
        {
            baseBuilder.NotNull(nameof(baseBuilder));

            // Configure Dependency
            var dependency = configureDependency.ConfigureDependency(baseBuilder);

            // Create Builder
            var storageBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new StorageBuilder(b, d)).Invoke(baseBuilder, dependency);

            // Configure Builder
            return storageBuilder
                .AddServices();
        }

    }
}
