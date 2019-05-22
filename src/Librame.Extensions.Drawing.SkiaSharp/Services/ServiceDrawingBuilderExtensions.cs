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

namespace Librame.Extensions.Drawing
{
    /// <summary>
    /// 服务图画构建器静态扩展。
    /// </summary>
    public static class ServiceDrawingBuilderExtensions
    {
        /// <summary>
        /// 添加服务集合。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDrawingBuilder"/>。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddServices(this IDrawingBuilder builder)
        {
            builder.Services.AddSingleton<ICaptchaService, InternalCaptchaService>();
            builder.Services.AddSingleton<IScaleService, InternalScaleService>();
            builder.Services.AddSingleton<IWatermarkService, InternalWatermarkService>();

            return builder;
        }

    }
}
