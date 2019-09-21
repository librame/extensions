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
        /// <param name="builderAction">给定的选项配置动作。</param>
        /// <param name="builderFactory">给定创建图画构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IExtensionBuilder builder,
            Action<DrawingBuilderOptions> builderAction,
            Func<IExtensionBuilder, IDrawingBuilder> builderFactory = null)
        {
            builderAction.NotNull(nameof(builderAction));

            return builder.AddDrawing(dependency =>
            {
                dependency.OptionsAction = builderAction;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加图画扩展。
        /// </summary>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建图画构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IExtensionBuilder builder,
            Action<DrawingBuilderDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, IDrawingBuilder> builderFactory = null)
            => builder.AddDrawing<DrawingBuilderDependencyOptions>(dependencyAction, builderFactory);

        /// <summary>
        /// 添加图画扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="dependencyAction">给定的依赖选项配置动作（可选）。</param>
        /// <param name="builderFactory">给定创建图画构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing<TDependencyOptions>(this IExtensionBuilder builder,
            Action<TDependencyOptions> dependencyAction = null,
            Func<IExtensionBuilder, IDrawingBuilder> builderFactory = null)
            where TDependencyOptions : DrawingBuilderDependencyOptions, new()
        {
            // Add Dependencies
            var dependency = dependencyAction.ConfigureDependencyOptions();

            // Add Builder
            builder.Services.OnlyConfigure(dependency.OptionsAction,
                dependency.OptionsName);

            var drawingBuilder = builderFactory.NotNullOrDefault(()
                => b => new DrawingBuilder(b)).Invoke(builder);

            return drawingBuilder
                .AddServices();
        }

    }
}
