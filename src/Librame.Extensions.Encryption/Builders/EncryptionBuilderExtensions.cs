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
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{EncryptionBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddEncryption(this IBuilder builder,
            Action<EncryptionBuilderOptions> configureOptions = null)
        {
            // Configure Options
            if (configureOptions != null)
                builder.Services.Configure(configureOptions);

            var encryptionBuilder = new InternalEncryptionBuilder(builder);

            return encryptionBuilder
                .AddBuffers()
                .AddConverters()
                .AddKeyGenerators()
                .AddServices();
        }

    }
}
