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
        /// <param name="configureOptions">给定的 <see cref="Action{DrawingBuilderOptions}"/>（可选；高优先级）。</param>
        /// <param name="configuration">给定的 <see cref="IConfiguration"/>（可选；次优先级）。</param>
        /// <param name="configureBinderOptions">给定的配置绑定器选项动作（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IBuilder builder,
            Action<DrawingBuilderOptions> configureOptions = null,
            IConfiguration configuration = null,
            Action<BinderOptions> configureBinderOptions = null)
        {
            var options = builder.Configure(configureOptions,
                configuration, configureBinderOptions);

            var drawingBuilder = new InternalDrawingBuilder(builder, options);

            return drawingBuilder
                .AddServices();
        }

    }
}
