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
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建图画构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        public static IDrawingBuilder AddDrawing(this IExtensionBuilder parentBuilder,
            Action<DrawingBuilderDependency> configureDependency = null,
            Func<IExtensionBuilder, DrawingBuilderDependency, IDrawingBuilder> builderFactory = null)
            => parentBuilder.AddDrawing<DrawingBuilderDependency>(configureDependency, builderFactory);

        /// <summary>
        /// 添加图画扩展。
        /// </summary>
        /// <typeparam name="TDependency">指定的依赖类型。</typeparam>
        /// <param name="parentBuilder">给定的父级 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="configureDependency">给定的配置依赖动作方法（可选）。</param>
        /// <param name="builderFactory">给定创建图画构建器的工厂方法（可选）。</param>
        /// <returns>返回 <see cref="IDrawingBuilder"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "parentBuilder")]
        public static IDrawingBuilder AddDrawing<TDependency>(this IExtensionBuilder parentBuilder,
            Action<TDependency> configureDependency = null,
            Func<IExtensionBuilder, TDependency, IDrawingBuilder> builderFactory = null)
            where TDependency : DrawingBuilderDependency, new()
        {
            // Configure Dependency
            var dependency = configureDependency.ConfigureDependency(parentBuilder);

            // Create Builder
            var drawingBuilder = builderFactory.NotNullOrDefault(()
                => (b, d) => new DrawingBuilder(b, d)).Invoke(parentBuilder, dependency);

            // Configure Builder
            return drawingBuilder
                .AddServices();
        }

    }
}
