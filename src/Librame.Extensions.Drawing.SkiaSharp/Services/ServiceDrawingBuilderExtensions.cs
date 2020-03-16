#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.DependencyInjection.Extensions;

namespace Librame.Extensions.Drawing.Builders
{
    using Services;

    static class ServiceDrawingBuilderExtensions
    {
        internal static IDrawingBuilder AddServices(this IDrawingBuilder builder)
        {
            builder.Services.TryAddSingleton<ICaptchaService, CaptchaService>();
            builder.Services.TryAddSingleton<IScaleService, ScaleService>();
            builder.Services.TryAddSingleton<IWatermarkService, WatermarkService>();

            return builder;
        }

    }
}
