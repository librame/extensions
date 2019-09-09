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
        /// <param name="setupAction">给定的选项配置动作（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption(this IExtensionBuilder builder,
            Action<EncryptionBuilderOptions> setupAction = null)
            => builder.AddEncryption(b => new EncryptionBuilder(b), setupAction);

        /// <summary>
        /// 添加加密扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="createFactory">给定创建加密构建器的工厂方法。</param>
        /// <param name="setupAction">给定的选项配置动作（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption(this IExtensionBuilder builder,
            Func<IExtensionBuilder, IEncryptionBuilder> createFactory,
            Action<EncryptionBuilderOptions> setupAction = null)
        {
            createFactory.NotNull(nameof(createFactory));

            // Add Builder
            builder.Services.OnlyConfigure(setupAction);

            var encryptionBuilder = createFactory.Invoke(builder);

            return encryptionBuilder
                .AddBuffers()
                .AddConverters()
                .AddKeyGenerators()
                .AddServices();
        }

    }
}
