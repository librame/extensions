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
        /// <param name="createFactory">给定创建图画构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IExtensionBuilder builder,
            Action<DrawingBuilderOptions> setupAction = null,
            Func<IExtensionBuilder, IDrawingBuilder> createFactory = null)
        {
            // Add Builder
            builder.Services.OnlyConfigure(setupAction);

            var drawingBuilder = (createFactory ??
                (b => new DrawingBuilder(b))).Invoke(builder);

            return drawingBuilder
                .AddServices();
        }

    }
}
