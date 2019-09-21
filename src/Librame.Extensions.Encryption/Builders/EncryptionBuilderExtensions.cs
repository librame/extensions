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

namespace Librame.Extensions.Encryption
{
    using Core;

    /// <summary>
    /// 加密构建器静态扩展。
    /// </summary>
    public static class EncryptionBuilderExtensions
    {
        /// <summary>
        /// 添加加密扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="builderAction">给定的选项配置动作。</param>
        /// <param name="builderFactory">给定创建加密构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption(this IExtensionBuilder builder,
            Action<EncryptionBuilderOptions> builderAction,
            Func<IExtensionBuilder, IEncryptionBuilder> builderFactory = null)
        {
            builderAction.NotNull(nameof(builderAction));

            return builder.AddEncryption(dependency =>
            {
                dependency.OptionsAction = builderAction;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加加密扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建加密构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption(this IExtensionBuilder builder,
            Action<EncryptionBuilderDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, IEncryptionBuilder> builderFactory = null)
            => builder.AddEncryption<EncryptionBuilderDependencyOptions>(dependencyAction, builderFactory);

        /// <summary>
        /// 添加加密扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建加密构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption<TDependencyOptions>(this IExtensionBuilder builder,
            Action<TDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, IEncryptionBuilder> builderFactory = null)
            where TDependencyOptions : EncryptionBuilderDependencyOptions, new()
        {
            // Add Dependencies
            var dependency = dependencyAction.ConfigureDependencyOptions();

            // Add Builder
            builder.Services.OnlyConfigure(dependency.OptionsAction,
                dependency.OptionsName);

            var encryptionBuilder = builderFactory.NotNullOrDefault(()
                => b => new EncryptionBuilder(b)).Invoke(builder);

            return encryptionBuilder
                .AddBuffers()
                .AddConverters()
                .AddKeyGenerators()
                .AddServices();
        }

    }
}
