﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 内部转换器核心构建器静态扩展。
    /// </summary>
    internal static class InternalConverterCoreBuilderExtensions
    {
        /// <summary>
        /// 添加转换器集合扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="ICoreBuilder"/>。</param>
        /// <returns>返回 <see cref="ICoreBuilder"/>。</returns>
        public static ICoreBuilder AddConverters(this ICoreBuilder builder)
        {
            builder.Services.AddSingleton<IEncodingConverter, EncodingConverter>();

            return builder;
        }

    }
}