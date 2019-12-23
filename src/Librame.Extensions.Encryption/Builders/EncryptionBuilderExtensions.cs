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
using Librame.Extensions.Encryption.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 加密构建器静态扩展。
    /// </summary>
    public static class EncryptionBuilderExtensions
    {
        /// <summary>
        /// 添加加密扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureOptions">给定的选项配置动作。</param>
        /// <param name="builderFactory">给定创建加密构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption(this IExtensionBuilder baseBuilder,
            Action<EncryptionBuilderOptions> configureOptions,
            Func<IExtensionBuilder, EncryptionBuilderDependency, IEncryptionBuilder> builderFactory = null)
        {
            configureOptions.NotNull(nameof(configureOptions));

            return baseBuilder.AddEncryption(dependency =>
            {
                dependency.Builder.ConfigureOptions = configureOptions;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加加密扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建加密构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption(this IExtensionBuilder baseBuilder,
            Action<EncryptionBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, EncryptionBuilderDependency, IEncryptionBuilder> builderFactory = null)
            => baseBuilder.AddEncryption<EncryptionBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加加密扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="baseBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建加密构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "baseBuilder")]
        public static IEncryptionBuilder AddEncryption<TDependencyOptions>(this IExtensionBuilder baseBuilder,
            Action<TDependencyOptions> configureDependency = null,
            Func<IExtensionBuilder, TDependencyOptions, IEncryptionBuilder> builderFactory = null)
            where TDependencyOptions : EncryptionBuilderDependency, new()
        {
            // Configure Dependency
            var dependency = configureDependency.ConfigureDependency(baseBuilder);

            // Create Builder
            var encryptionBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new EncryptionBuilder(b, d)).Invoke(baseBuilder, dependency);

            // Configure Builder
            return encryptionBuilder
                .AddKeyGenerators()
                .AddServices();
        }

    }
}
