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

namespace Librame.Builders
{
    using Buffers;

    /// <summary>
    /// 缓冲区构建器静态扩展。
    /// </summary>
    public static class BufferBuilderExtensions
    {

        /// <summary>
        /// 注册缓冲区集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddBuffers(this IBuilder builder)
        {
            builder.Services.AddTransient(typeof(IReadOnlyBuffer<>), typeof(DefaultReadOnlyBuffer<>));
            builder.Services.AddTransient(typeof(IBuffer<>), typeof(DefaultBuffer<>));

            return builder;
        }

    }
}
