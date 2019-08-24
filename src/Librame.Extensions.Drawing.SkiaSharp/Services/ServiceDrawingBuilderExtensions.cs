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
    static class ServiceDrawingBuilderExtensions
    {
        public static IDrawingBuilder AddServices(this IDrawingBuilder builder)
        {
            builder.Services.AddScoped<ICaptchaService, CaptchaService>();
            builder.Services.AddScoped<IScaleService, ScaleService>();
            builder.Services.AddScoped<IWatermarkService, WatermarkService>();

            return builder;
        }

    }
}
