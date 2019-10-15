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
using Librame.Extensions.Core;
using Librame.Extensions.Storage;
using System;

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
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="builderAction">给定的选项配置动作。</param>
        /// <param name="builderFactory">给定创建存储构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IExtensionBuilder builder,
            Action<StorageBuilderOptions> builderAction,
            Func<IExtensionBuilder, StorageBuilderDependencyOptions, IStorageBuilder> builderFactory = null)
        {
            builderAction.NotNull(nameof(builderAction));

            return builder.AddStorage(dependency =>
            {
                dependency.Builder.Action = builderAction;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加存储扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建存储构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IExtensionBuilder builder,
            Action<StorageBuilderDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, StorageBuilderDependencyOptions, IStorageBuilder> builderFactory = null)
            => builder.AddStorage<StorageBuilderDependencyOptions>(dependencyAction, builderFactory);

        /// <summary>
        /// 添加存储扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建存储构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage<TDependencyOptions>(this IExtensionBuilder builder,
            Action<TDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, TDependencyOptions, IStorageBuilder> builderFactory = null)
            where TDependencyOptions : StorageBuilderDependencyOptions, new()
        {
            // Configure DependencyOptions
            var dependency = dependencyAction.ConfigureDependency();
            builder.Services.AddAllOptionsConfigurators(dependency);

            // Create Builder
            var storageBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new StorageBuilder(b, d)).Invoke(builder, dependency);

            // Configure Builder
            return storageBuilder
                .AddServices();
        }

    }
}
