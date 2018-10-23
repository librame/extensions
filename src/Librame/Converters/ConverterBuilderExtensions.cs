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
    using Converters;

    /// <summary>
    /// 转换器构建器静态扩展。
    /// </summary>
    public static class ConverterBuilderExtensions
    {

        /// <summary>
        /// 注册转换器集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddConverters(this IBuilder builder)
        {
            builder.Services.AddSingleton<IEncodingConverter, DefaultEncodingConverter>();

            return builder;
        }

    }
}
