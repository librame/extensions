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

namespace Librame.Extensions.Encryption
{
    /// <summary>
    /// 内部服务加密构建器静态扩展。
    /// </summary>
    internal static class InternalServiceEncryptionBuilderExtensions
    {
        /// <summary>
        /// 添加服务集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddServices(this IEncryptionBuilder builder)
        {
            builder.Services.AddScoped<IHashService, InternalHashService>();
            builder.Services.AddScoped<IKeyedHashService, InternalKeyedHashService>();
            builder.Services.AddScoped<IRsaService, InternalRsaService>();
            builder.Services.AddScoped<ISymmetricService, InternalSymmetricService>();

            return builder;
        }

    }
}
