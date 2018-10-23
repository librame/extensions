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
    using Locators;

    /// <summary>
    /// 图画构建器静态扩展。
    /// </summary>
    public static class DrawingBuilderExtensions
    {

        /// <summary>
        /// 添加图画。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{IDrawingBuilderOptions}"/>。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IBuilder builder,
            IConfiguration configuration = null, Action<IDrawingBuilderOptions> configureOptions = null)
        {
            return builder.AddDrawing<DefaultDrawingBuilderOptions>(configuration, configureOptions);
        }
        /// <summary>
        /// 添加图画。
        /// </summary>
        /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IBuilder"/>。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>。</param>
        /// <param name="configureOptions">给定的 <see cref="Action{TBuilderOptions}"/>。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing<TBuilderOptions>(this IBuilder builder,
            IConfiguration configuration = null, Action<TBuilderOptions> configureOptions = null)
            where TBuilderOptions : class, IDrawingBuilderOptions
        {
            if (configuration.IsNotDefault())
                builder.Services.Configure<TBuilderOptions>(configuration);

            if (configureOptions.IsDefault())
                configureOptions = (options) => { };

            builder.Services.Configure<TBuilderOptions>(options =>
            {
                var fontFileLocator = new DefaultFileLocator("font.ttf");
                options.Captcha.Font.FileLocator = fontFileLocator;
                options.Watermark.Font.FileLocator = fontFileLocator;
                options.Watermark.ImageFileLocator = new DefaultFileLocator("watermark.png");

                configureOptions.Invoke(options);
            });

            var drawingBuilder = builder.AsDrawingBuilder();

            drawingBuilder.AddCaptcha()
                .AddScale()
                .AddWatermark();

            return drawingBuilder;
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
        public static IDrawingBuilder AddCaptcha(this IDrawingBuilder builder)
        {
            builder.Services.AddSingleton<ICaptchaService, InternalCaptchaService>();

            return builder;
        }

        /// <summary>
        /// 添加缩放。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDrawingBuilder"/>。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddScale(this IDrawingBuilder builder)
        {
            builder.Services.AddSingleton<IScaleService, InternalScaleService>();

            return builder;
        }

        /// <summary>
        /// 添加水印。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IDrawingBuilder"/>。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddWatermark(this IDrawingBuilder builder)
        {
            builder.Services.AddSingleton<IWatermarkService, InternalWatermarkService>();

            return builder;
        }

    }
}
