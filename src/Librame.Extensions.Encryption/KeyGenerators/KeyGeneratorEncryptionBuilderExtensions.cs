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
    /// 密钥生成器加密构建器静态扩展。
    /// </summary>
    public static class KeyGeneratorEncryptionBuilderExtensions
    {
        /// <summary>
        /// 添加密钥生成器集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddKeyGenerators(this IEncryptionBuilder builder)
        {
            builder.Services.AddScoped<IKeyGenerator, InternalKeyGenerator>();

            return builder;
        }

    }
}
