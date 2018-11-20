#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace Librame.Builders
{
    using Extensions;
    using Extensions.Drawing;

    /// <summary>
    /// 图画构建器静态扩展。
    /// </summary>
    public static class DrawingBuilderExtensions
    {

        /// <summary>
        /// 添加图画扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="builderOptions">给定的 <see cref="DrawingBuilderOptions"/>（可选）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="postConfigureOptions">给定的 <see cref="Action{DrawingBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IBuilder builder, DrawingBuilderOptions builderOptions = null,
            IConfiguration configuration = null, Action<DrawingBuilderOptions> postConfigureOptions = null)
        {
            return builder.AddDrawing<DrawingBuilderOptions>(builderOptions ?? new DrawingBuilderOptions(),
                configuration, postConfigureOptions);
        }
        /// <summary>
        /// 添加图画扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="builderOptions">给定的构建器选项。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选）。</param>
        /// <param name="postConfigureOptions">给定的 <see cref="Action{TBuilderOptions}"/>（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing<TBuilderOptions>(this IBuilder builder, TBuilderOptions builderOptions,
            IConfiguration configuration = null, Action<TBuilderOptions> postConfigureOptions = null)
            where TBuilderOptions : DrawingBuilderOptions
        {
            var fontFileLocator = "font.ttf".AsFileLocator();

            if (builderOptions.Captcha.Font.FileLocator.IsDefault())
                builderOptions.Captcha.Font.FileLocator = fontFileLocator;

            if (builderOptions.Watermark.Font.FileLocator.IsDefault())
                builderOptions.Watermark.Font.FileLocator = fontFileLocator;

            if (builderOptions.Watermark.ImageFileLocator.IsDefault())
                builderOptions.Watermark.ImageFileLocator = "watermark.png".AsFileLocator();

            return builder.AddBuilder(b =>
            {
                return b.AsDrawingBuilder()
                    .AddCaptchas()
                    .AddScales()
                    .AddWatermarks();
            },
            typeof(DrawingBuilderOptions), builderOptions, configuration, postConfigureOptions);
        }


        /// <summary>
        /// 转换为内部图画构建器。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AsDrawingBuilder(this IBuilder builder)
        {
            return new InternalDrawingBuilder(builder);
        }

        /// <summary>
        /// 添加验证码。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDrawingBuilder"/>。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddCaptchas(this IDrawingBuilder builder)
        {
            builder.Services.AddSingleton<ICaptchaService, InternalCaptchaService>();

            return builder;
        }

        /// <summary>
        /// 添加缩放。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDrawingBuilder"/>。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddScales(this IDrawingBuilder builder)
        {
            builder.Services.AddSingleton<IScaleService, InternalScaleService>();

            return builder;
        }

        /// <summary>
        /// 添加水印。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDrawingBuilder"/>。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddWatermarks(this IDrawingBuilder builder)
        {
            builder.Services.AddSingleton<IWatermarkService, InternalWatermarkService>();

            return builder;
        }

    }
}
