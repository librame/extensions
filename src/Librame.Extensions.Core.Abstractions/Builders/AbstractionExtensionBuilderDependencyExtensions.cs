#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;

namespace Librame.Extensions.Core.Builders
{
    /// <summary>
    /// <see cref="IExtensionBuilderDependency"/> 静态扩展。
    /// </summary>
    public static class AbstractionExtensionBuilderDependencyExtensions
    {
        /// <summary>
        /// 获取必需的构建器依赖实例（支持查找当前或父级实例）。
        /// </summary>
        /// <typeparam name="TDependency">指定的构建器依赖类型。</typeparam>
        /// <param name="currentDependency">给定的当前 <see cref="IExtensionBuilderDependency"/>。</param>
        /// <param name="excludeCurrentDependency">排除当前 <see cref="IExtensionBuilderDependency"/>（可选；默认不排除）。</param>
        /// <returns>返回 <typeparamref name="TDependency"/> 或抛出 <see cref="InvalidOperationException"/>。</returns>
        public static TDependency GetRequiredDependency<TDependency>
            (this IExtensionBuilderDependency currentDependency, bool excludeCurrentDependency = false)
            where TDependency : class, IExtensionBuilderDependency
        {
            if (!currentDependency.TryGetDependency<TDependency>(out var resultDependency, excludeCurrentDependency))
                throw new InvalidOperationException($"The current dependency is not or does not contain a parent dependency '{typeof(TDependency)}'.");

            return resultDependency;
        }

        /// <summary>
        /// 包含指定构建器依赖类型的实例（支持查找当前或父级实例）。
        /// </summary>
        /// <typeparam name="TDependency">指定的构建器类型。</typeparam>
        /// <param name="currentDependency">给定的当前 <see cref="IExtensionBuilderDependency"/>。</param>
        /// <param name="excludeCurrentDependency">排除当前 <see cref="IExtensionBuilderDependency"/>（可选；默认不排除）。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool ContainsDependency<TDependency>(this IExtensionBuilderDependency currentDependency,
            bool excludeCurrentDependency = false)
            where TDependency : class, IExtensionBuilderDependency
            => currentDependency.TryGetDependency<TDependency>(out _, excludeCurrentDependency);

        /// <summary>
        /// 尝试获取指定构建器依赖类型的实例（支持查找当前或父级实例）。
        /// </summary>
        /// <typeparam name="TDependency">指定的构建器类型。</typeparam>
        /// <param name="currentDependency">给定的当前 <see cref="IExtensionBuilderDependency"/>。</param>
        /// <param name="resultDependency">输出结果 <see cref="IExtensionBuilderDependency"/>。</param>
        /// <param name="excludeCurrentDependency">排除当前 <see cref="IExtensionBuilderDependency"/>（可选；默认不排除）。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetDependency<TDependency>(this IExtensionBuilderDependency currentDependency,
            out TDependency resultDependency, bool excludeCurrentDependency = false)
            where TDependency : class, IExtensionBuilderDependency
        {
            resultDependency = LookupDependency(excludeCurrentDependency
                ? currentDependency?.ParentDependency
                : currentDependency);

            return resultDependency.IsNotNull();

            // LookupDependency
            TDependency LookupDependency(IExtensionBuilderDependency builderDependency)
            {
                if (builderDependency.IsNull())
                    return null;

                if (builderDependency is TDependency dependency)
                    return dependency;

                return LookupDependency(builderDependency.ParentDependency);
            }
        }


        /// <summary>
        /// 枚举当前以及父级构建器依赖字典集合。
        /// </summary>
        /// <param name="currentDependency">给定的当前 <see cref="IExtensionBuilderDependency"/>。</param>
        /// <param name="excludeCurrentDependency">排除当前 <see cref="IExtensionBuilderDependency"/>（可选；默认不排除）。</param>
        /// <returns>返回字典集合。</returns>
        public static Dictionary<string, IExtensionBuilderDependency> EnumerateDependencies
            (this IExtensionBuilderDependency currentDependency, bool excludeCurrentDependency = false)
        {
            var dependencies = new Dictionary<string, IExtensionBuilderDependency>();

            LookupDependency(excludeCurrentDependency
                ? currentDependency?.ParentDependency
                : currentDependency);

            return dependencies;

            // LookupDependency
            void LookupDependency(IExtensionBuilderDependency dependency)
            {
                if (dependency.IsNull())
                    return;

                dependencies.Add(dependency.Name, dependency);

                // 链式查找父级构建器依赖
                LookupDependency(dependency.ParentDependency);
            }
        }

    }
}
