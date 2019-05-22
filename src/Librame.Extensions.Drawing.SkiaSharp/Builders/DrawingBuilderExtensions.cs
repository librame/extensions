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
using System;

namespace Librame.Extensions.Drawing
{
    using Core;

    /// <summary>
    /// 图画构建器静态扩展。
    /// </summary>
    public static class DrawingBuilderExtensions
    {
        /// <summary>
        /// 添加图画扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{DrawingBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IBuilder builder,
            Action<DrawingBuilderOptions> configureOptions = null)
        {
            var fontFileLocator = "font.ttf".AsFileLocator();
            Action<DrawingBuilderOptions> _configureOptions = options =>
            {
                options.Captcha.Font.FileLocator = fontFileLocator;
                options.Watermark.Font.FileLocator = fontFileLocator;
                options.Watermark.ImageFileLocator = "watermark.png".AsFileLocator();

                configureOptions?.Invoke(options);
            };

            // Configure Options
            builder.Services.Configure(_configureOptions);

            var drawingBuilder = new InternalDrawingBuilder(builder);

            return drawingBuilder
                .AddServices();
        }

    }
}
