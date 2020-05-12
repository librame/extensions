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
using Librame.Extensions.Core.Options;
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
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建存储构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        public static IStorageBuilder AddStorage(this IExtensionBuilder parentBuilder,
            Action<StorageBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, StorageBuilderDependency, IStorageBuilder> builderFactory = null)
            => parentBuilder.AddStorage<StorageBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加存储扩展。
        /// </summary>
        /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建存储构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IStorageBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IStorageBuilder AddStorage<TDependency>(this IExtensionBuilder parentBuilder,
            Action<TDependency> configureDependency = null,
            Func<IExtensionBuilder, TDependency, IStorageBuilder> builderFactory = null)
            where TDependency : StorageBuilderDependency
        {
            // Clear Options Cache
            ConsistencyOptionsCache.TryRemove<StorageBuilderOptions>();

            // Add Builder Dependency
            var dependency = parentBuilder.AddBuilderDependency(out var dependencyType, configureDependency);
            parentBuilder.Services.TryAddReferenceBuilderDependency<StorageBuilderDependency>(dependency, dependencyType);

            // Create Builder
            var storageBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new StorageBuilder(b, d)).Invoke(parentBuilder, dependency);

            // Configure Builder
            return storageBuilder
                .AddServices();
        }

    }
}
