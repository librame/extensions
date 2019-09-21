#region License

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

namespace Librame.Extensions.Storage
{
    using Core;

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
            Func<IExtensionBuilder, IStorageBuilder> builderFactory = null)
        {
            builderAction.NotNull(nameof(builderAction));

            return builder.AddStorage(dependency =>
            {
                dependency.OptionsAction = builderAction;
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
            Func<IExtensionBuilder, IStorageBuilder> builderFactory = null)
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
            Func<IExtensionBuilder, IStorageBuilder> builderFactory = null)
            where TDependencyOptions : StorageBuilderDependencyOptions, new()
        {
            // Add Dependencies
            var dependency = dependencyAction.ConfigureDependencyOptions();

            // Add Builder
            builder.Services.OnlyConfigure(dependency.OptionsAction,
                dependency.OptionsName);

            var storageBuilder = builderFactory.NotNullOrDefault(()
                => b => new StorageBuilder(b)).Invoke(builder);

            return storageBuilder
                .AddServices();
        }

    }
}
