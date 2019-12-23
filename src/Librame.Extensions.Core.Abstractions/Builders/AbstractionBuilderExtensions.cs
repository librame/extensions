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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Core.Builders
{
    /// <summary>
    /// 抽象构建器静态扩展。
    /// </summary>
    public static class AbstractionBuilderExtensions
    {
        /// <summary>
        /// 包含指定父级构建器类型的实例。
        /// </summary>
        /// <typeparam name="TParentBuilder">指定的父级构建器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool ContainsParentBuilder<TParentBuilder>(this IExtensionBuilder builder)
            where TParentBuilder : IExtensionBuilder
            => builder.TryGetParentBuilder<TParentBuilder>(out _);

        /// <summary>
        /// 尝试获取指定父级构建器类型的实例。
        /// </summary>
        /// <typeparam name="TParentBuilder">指定的父级构建器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="parentBuilder">输出父级 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetParentBuilder<TParentBuilder>(this IExtensionBuilder builder,
            out TParentBuilder parentBuilder)
            where TParentBuilder : IExtensionBuilder
        {
            if (builder.IsNull())
            {
                parentBuilder = default;
                return false;
            }

            if (builder.ParentBuilder is TParentBuilder _builder)
            {
                parentBuilder = _builder;
                return true;
            }

            return builder.ParentBuilder.TryGetParentBuilder(out parentBuilder);
        }


        #region ExportDependencies

        /// <summary>
        /// 导出扩展构建器依赖集合。
        /// </summary>
        /// <param name="provider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="services">给定的 <see cref="IServiceCollection"/>。</param>
        /// <returns>返回 <see cref="Dictionary{String, IExtensionBuilderDependency}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static Dictionary<string, IExtensionBuilderDependency> ExportDependencies(this IServiceProvider provider,
            IServiceCollection services)
        {
            provider.NotNull(nameof(provider));
            services.NotEmpty(nameof(services));

            IExtensionBuilder lastBuilder = null;

            var baseBuilderType = typeof(IExtensionBuilder);
            foreach (var service in services.Reverse())
            {
                if (service.ServiceType.IsAssignableToBaseType(baseBuilderType))
                {
                    lastBuilder = (IExtensionBuilder)provider.GetService(service.ServiceType);
                    break;
                }
            }

            return lastBuilder.ExportDependencies();
        }

        /// <summary>
        /// 导出扩展构建器依赖集合。
        /// </summary>
        /// <typeparam name="TLastBuilder">指定的末尾扩展构建器类型。</typeparam>
        /// <param name="provider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <returns>返回 <see cref="Dictionary{String, IExtensionBuilderDependency}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "provider")]
        public static Dictionary<string, IExtensionBuilderDependency> ExportDependencies<TLastBuilder>(this IServiceProvider provider)
            where TLastBuilder : IExtensionBuilder
        {
            provider.NotNull(nameof(provider));

            var lastBuilder = provider.GetRequiredService<TLastBuilder>();
            return lastBuilder.ExportDependencies();
        }

        /// <summary>
        /// 导出扩展构建器依赖集合。
        /// </summary>
        /// <param name="provider">给定的 <see cref="IServiceProvider"/>。</param>
        /// <param name="lastBuilderType">给定的末尾扩展构建器类型（需实现 <see cref="IExtensionBuilder"/>）。</param>
        /// <returns>返回 <see cref="Dictionary{String, IExtensionBuilderDependency}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "provider")]
        public static Dictionary<string, IExtensionBuilderDependency> ExportDependencies(this IServiceProvider provider,
            Type lastBuilderType)
        {
            provider.NotNull(nameof(provider));

            if (!lastBuilderType.IsAssignableToBaseType(typeof(IExtensionBuilder)))
                throw new ArgumentException($"Invalid last extension builder type '{lastBuilderType}', {nameof(IExtensionBuilder)} interface is not implemented.");

            var lastBuilder = (IExtensionBuilder)provider.GetService(lastBuilderType);
            return lastBuilder?.ExportDependencies();
        }

        /// <summary>
        /// 导出扩展构建器依赖集合。
        /// </summary>
        /// <param name="lastBuilder">给定的末尾 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <see cref="Dictionary{String, IExtensionBuilderDependency}"/>。</returns>
        public static Dictionary<string, IExtensionBuilderDependency> ExportDependencies(this IExtensionBuilder lastBuilder)
        {
            var dependencies = new Dictionary<string, IExtensionBuilderDependency>();

            PopulateDependencies(lastBuilder, dependencies);

            return dependencies.Reverse().ToDictionary(pair => pair.Key, pair => pair.Value);
        }

        private static void PopulateDependencies(IExtensionBuilder builder,
            Dictionary<string, IExtensionBuilderDependency> dependencies)
        {
            if (builder.IsNull())
                return;

            var key = GetDependencyKey(builder.Dependency);
            if (key.IsNotEmpty() && !dependencies.ContainsKey(key))
                dependencies.Add(key, builder.Dependency);
            else
                dependencies[key] = builder.Dependency;

            PopulateDependencies(builder.ParentBuilder, dependencies);
        }

        private static string GetDependencyKey(IExtensionBuilderDependency dependency)
            => dependency?.GetType().Name;

        #endregion

    }
}
