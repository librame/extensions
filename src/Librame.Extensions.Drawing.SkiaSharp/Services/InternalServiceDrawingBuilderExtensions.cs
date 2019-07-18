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

namespace Librame.Extensions.Drawing
{
    /// <summary>
    /// 内部服务图画构建器静态扩展。
    /// </summary>
    internal static class InternalServiceDrawingBuilderExtensions
    {
        /// <summary>
        /// 添加服务集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDrawingBuilder"/>。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddServices(this IDrawingBuilder builder)
        {
            builder.Services.AddScoped<ICaptchaService, InternalCaptchaService>();
            builder.Services.AddScoped<IScaleService, InternalScaleService>();
            builder.Services.AddScoped<IWatermarkService, InternalWatermarkService>();

            return builder;
        }

    }
}