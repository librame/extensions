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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 验证器构建器静态扩展。
    /// </summary>
    public static class ValidatorBuilderExtensions
    {
        /// <summary>
        /// 添加验证器集合扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddValidators(this IBuilder builder)
        {
            builder.Services.AddSingleton<IStringValidator, StringValidator>();

            return builder;
        }

    }
}
