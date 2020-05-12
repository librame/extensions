#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Generic;

namespace Librame.Extensions.Core.Builders
{
    /// <summary>
    /// 抽象构建器静态扩展。
    /// </summary>
    public static class AbstractionBuilderExtensions
    {

        #region Builders

        /// <summary>
        /// 获取必须的构建器依赖。
        /// </summary>
        /// <typeparam name="TDependency">指定的构建器依赖类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <typeparamref name="TDependency"/>。</returns>
        public static TDependency GetRequiredDependency<TDependency>(this IExtensionBuilder builder)
            where TDependency : class, IExtensionBuilderDependency
        {
            if (!builder.TryGetDependency(out TDependency result))
                throw new InvalidOperationException($"The builder's dependency '{builder.Dependency?.GetType()}' is not '{typeof(TDependency)}'.");

            return result;
        }

        /// <summary>
        /// 尝试获取指定类型的构建器依赖。
        /// </summary>
        /// <typeparam name="TDependency">指定的构建器依赖类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="result">输出 <typeparamref name="TDependency"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGetDependency<TDependency>(this IExtensionBuilder builder, out TDependency result)
            where TDependency : class, IExtensionBuilderDependency
        {
            builder.NotNull(nameof(builder));

            if (builder?.Dependency is TDependency dependency)
            {
                result = dependency;
                return true;
            }

            result = null;
            return false;
        }


        /// <summary>
        /// 获取必需的父级构建器。
        /// </summary>
        /// <typeparam name="TParentBuilder">指定的父级构建器类型。</typeparam>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <typeparamref name="TParentBuilder"/> 或抛出 <see cref="InvalidOperationException"/>。</returns>
        public static TParentBuilder GetRequiredParentBuilder<TParentBuilder>
            (this IExtensionBuilder dependency)
            where TParentBuilder : class, IExtensionBuilder
        {
            if (!dependency.TryGetParentBuilder<TParentBuilder>(out var result))
                throw new InvalidOperationException($"The builder is not contains parent builder '{typeof(TParentBuilder)}'.");

            return result;
        }

        /// <summary>
        /// 包含指定父级构建器类型的实例。
        /// </summary>
        /// <typeparam name="TParentBuilder">指定的父级构建器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool ContainsParentBuilder<TParentBuilder>(this IExtensionBuilder builder)
            where TParentBuilder : class, IExtensionBuilder
            => builder.TryGetParentBuilder<TParentBuilder>(out _);

        /// <summary>
        /// 尝试获取指定父级构建器类型的实例。
        /// </summary>
        /// <typeparam name="TParentBuilder">指定的父级构建器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="result">输出父级 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetParentBuilder<TParentBuilder>(this IExtensionBuilder builder,
            out TParentBuilder result)
            where TParentBuilder : class, IExtensionBuilder
        {
            // 仅查找父级
            result = GetParentBuilder(builder?.ParentBuilder);
            return result.IsNotNull();

            // GetParentBuilder
            TParentBuilder GetParentBuilder(IExtensionBuilder currentBuilder)
            {
                if (currentBuilder.IsNull())
                    return null;

                if (currentBuilder is TParentBuilder parentBuilder)
                    return parentBuilder;

                return GetParentBuilder(currentBuilder.ParentBuilder);
            }
        }

        #endregion


        #region Dependencies

        /// <summary>
        /// 获取必需的父级构建器依赖。
        /// </summary>
        /// <typeparam name="TParentDependency">指定的父级构建器依赖类型。</typeparam>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        /// <returns>返回 <typeparamref name="TParentDependency"/> 或抛出 <see cref="InvalidOperationException"/>。</returns>
        public static TParentDependency GetRequiredParentDependency<TParentDependency>
            (this IExtensionBuilderDependency dependency)
            where TParentDependency : class, IExtensionBuilderDependency
        {
            if (!dependency.TryGetParentDependency<TParentDependency>(out var result))
                throw new InvalidOperationException($"The dependency is not contains parent dependency '{typeof(TParentDependency)}'.");

            return result;
        }

        /// <summary>
        /// 包含指定父级构建器依赖类型的实例。
        /// </summary>
        /// <typeparam name="TParentDependency">指定的父级构建器类型。</typeparam>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool ContainsParentDependency<TParentDependency>(this IExtensionBuilderDependency dependency)
            where TParentDependency : class, IExtensionBuilderDependency
            => dependency.TryGetParentDependency<TParentDependency>(out _);

        /// <summary>
        /// 尝试获取指定父级构建器依赖类型的实例。
        /// </summary>
        /// <typeparam name="TParentDependency">指定的父级构建器类型。</typeparam>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        /// <param name="result">输出父级 <see cref="IExtensionBuilderDependency"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetParentDependency<TParentDependency>(this IExtensionBuilderDependency dependency,
            out TParentDependency result)
            where TParentDependency : class, IExtensionBuilderDependency
        {
            // 仅查找父级
            result = GetParentDependency(dependency?.ParentDependency);
            return result.IsNotNull();

            // GetParentDependency
            TParentDependency GetParentDependency(IExtensionBuilderDependency currentDependency)
            {
                if (currentDependency.IsNull())
                    return null;

                if (currentDependency is TParentDependency parentDependency)
                    return parentDependency;

                return GetParentDependency(currentDependency.ParentDependency);
            }
        }


        /// <summary>
        /// 枚举当前以及所有父级依赖字典集合。
        /// </summary>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilderDependency"/>。</param>
        /// <returns>返回字典集合。</returns>
        public static Dictionary<string, IExtensionBuilderDependency> EnumerateDependencies
            (this IExtensionBuilderDependency dependency)
        {
            var dependencies = new Dictionary<string, IExtensionBuilderDependency>();

            AddParentDependency(dependency);

            return dependencies;

            // AddParentDependency
            void AddParentDependency(IExtensionBuilderDependency currentDependency)
            {
                if (currentDependency.IsNull())
                    return;

                dependencies.Add(currentDependency.Name, currentDependency);

                // 链式添加父级依赖
                AddParentDependency(currentDependency.ParentDependency);
            }
        }

        #endregion

    }
}
