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
using Microsoft.Extensions.Localization;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 本地化构建器静态扩展。
    /// </summary>
    public static class LocalizationBuilderExtensions
    {

        /// <summary>
        /// 添加本地化集合扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IBuilder"/>。</returns>
        public static IBuilder AddLocalizations(this IBuilder builder)
        {
            builder.Services.AddScoped(typeof(IExpressionStringLocalizer<>), typeof(ExpressionStringLocalizer<>));
            builder.Services.TryReplace<IStringLocalizerFactory, ExpressionStringLocalizerFactory>();

            return builder;
        }

    }
}
