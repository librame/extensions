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
    /// 内部缓冲区加密构建器静态扩展。
    /// </summary>
    internal static class InternalBufferEncryptionBuilderExtensions
    {
        /// <summary>
        /// 添加缓冲区集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IEncryptionBuilder"/>。</param>
        /// <returns>返回 <see cref="IEncryptionBuilder"/>。</returns>
        public static IEncryptionBuilder AddBuffers(this IEncryptionBuilder builder)
        {
            builder.Services.AddScoped(typeof(IEncryptionBuffer<,>), typeof(InternalEncryptionBuffer<,>));

            return builder;
        }

    }
}
