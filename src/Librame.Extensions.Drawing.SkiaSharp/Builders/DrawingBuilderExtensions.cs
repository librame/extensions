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
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="setupAction">给定的选项配置动作（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IExtensionBuilder builder,
            Action<DrawingBuilderOptions> setupAction = null)
        {
            return builder.AddDrawing(b => new DrawingBuilder(b), setupAction);
        }

        /// <summary>
        /// 添加图画扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="createFactory">给定创建图画构建器的工厂方法。</param>
        /// <param name="setupAction">给定的选项配置动作（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IExtensionBuilder builder,
            Func<IExtensionBuilder, IDrawingBuilder> createFactory,
            Action<DrawingBuilderOptions> setupAction = null)
        {
            createFactory.NotNull(nameof(createFactory));

            // Add Builder
            builder.Services.OnlyConfigure(setupAction);

            var drawingBuilder = createFactory.Invoke(builder);

            return drawingBuilder
                .AddServices();
        }

    }
}
