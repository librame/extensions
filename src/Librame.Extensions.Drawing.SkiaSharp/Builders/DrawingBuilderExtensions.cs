﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame.Extensions;
using Librame.Extensions.Core.Builders;
using Librame.Extensions.Core.Options;
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IDrawingBuilder AddDrawing<TDependency>(this IExtensionBuilder parentBuilder,
            Action<TDependency> configureDependency = null,
            Func<IExtensionBuilder, TDependency, IDrawingBuilder> builderFactory = null)
            where TDependency : DrawingBuilderDependency
        {
            // Clear Options Cache
            ConsistencyOptionsCache.TryRemove<DrawingBuilderOptions>();

            // Add Builder Dependency
            var dependency = parentBuilder.AddBuilderDependency(out var dependencyType, configureDependency);
            parentBuilder.Services.TryAddReferenceBuilderDependency<DrawingBuilderDependency>(dependency, dependencyType);

            // Create Builder
            return builderFactory.NotNullOrDefault(()
                => (b, d) => new DrawingBuilder(b, d)).Invoke(parentBuilder, dependency);
        }

    }
}
