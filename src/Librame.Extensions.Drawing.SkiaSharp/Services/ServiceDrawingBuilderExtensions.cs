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
            builder.Services.TryAddScoped<ICaptchaService, CaptchaService>();
            builder.Services.TryAddScoped<IScaleService, ScaleService>();
            builder.Services.TryAddScoped<IWatermarkService, WatermarkService>();

            return builder;
        }

    }
}
