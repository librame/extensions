﻿#region License

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
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建加密构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption(this IExtensionBuilder parentBuilder,
            Action<EncryptionBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, EncryptionBuilderDependency, IEncryptionBuilder> builderFactory = null)
            => parentBuilder.AddEncryption<EncryptionBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加加密扩展。
        /// </summary>
        /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建加密构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IEncryptionBuilder AddEncryption<TDependency>(this IExtensionBuilder parentBuilder,
            Action<TDependency> configureDependency = null,
            Func<IExtensionBuilder, TDependency, IEncryptionBuilder> builderFactory = null)
            where TDependency : EncryptionBuilderDependency
        {
            // Configure Dependency
            var dependency = configureDependency.ConfigureDependency(parentBuilder);

            // Create Builder
            var encryptionBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new EncryptionBuilder(b, d)).Invoke(parentBuilder, dependency);

            // Configure Builder
            return encryptionBuilder
                .AddKeyGenerators()
                .AddServices();
        }

    }
}
