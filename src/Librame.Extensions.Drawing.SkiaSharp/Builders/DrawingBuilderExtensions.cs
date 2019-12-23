#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core.Builders;
using Librame.Extensions.Drawing.Builders;
using System;
using System.Diagnostics.CodeAnalysis;

namespace Microsoft.Extensions.DependencyInjection
{
    /// <summary>
    /// 图画构建器静态扩展。
    /// </summary>
    public static class DrawingBuilderExtensions
    {
        /// <summary>
        /// 添加图画扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureOptions">给定的配置选项动作方法。</param>
        /// <param name="builderFactory">给定创建图画构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IExtensionBuilder baseBuilder,
            Action<DrawingBuilderOptions> configureOptions,
            Func<IExtensionBuilder, DrawingBuilderDependency, IDrawingBuilder> builderFactory = null)
        {
            configureOptions.NotNull(nameof(configureOptions));

            return baseBuilder.AddDrawing(dependency =>
            {
                dependency.Builder.ConfigureOptions = configureOptions;
            },
            builderFactory);
        }

        /// <summary>
        /// 添加图画扩展。
        /// </summary>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建图画构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IExtensionBuilder baseBuilder,
            Action<DrawingBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, DrawingBuilderDependency, IDrawingBuilder> builderFactory = null)
            => baseBuilder.AddDrawing<DrawingBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加图画扩展。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖类型。</typeparam>
        /// <param name="baseBuilder">给定的基础 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建图画构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "baseBuilder")]
        public static IDrawingBuilder AddDrawing<TDependencyOptions>(this IExtensionBuilder baseBuilder,
            Action<TDependencyOptions> configureDependency = null,
            Func<IExtensionBuilder, TDependencyOptions, IDrawingBuilder> builderFactory = null)
            where TDependencyOptions : DrawingBuilderDependency, new()
        {
            // Configure Dependency
            var dependency = configureDependency.ConfigureDependency(baseBuilder);

            // Create Builder
            var drawingBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new DrawingBuilder(b, d)).Invoke(baseBuilder, dependency);

            // Configure Builder
            return drawingBuilder
                .AddServices();
        }

    }
}
