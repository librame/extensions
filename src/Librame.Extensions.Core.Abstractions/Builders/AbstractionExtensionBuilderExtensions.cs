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

namespace Librame.Extensions.Core.Builders
{
    /// <summary>
    /// <see cref="IExtensionBuilder"/> 静态扩展。
    /// </summary>
    public static class AbstractionExtensionBuilderExtensions
    {

        #region ParentBuilder

        /// <summary>
        /// 获取必需的父级构建器。
        /// </summary>
        /// <typeparam name="TParentBuilder">指定的父级构建器类型。</typeparam>
        /// <param name="dependency">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回 <typeparamref name="TParentBuilder"/> 或抛出 <see cref="InvalidOperationException"/>。</returns>
        public static TParentBuilder GetRequiredParentBuilder<TParentBuilder>(this IExtensionBuilder dependency)
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

    }
}
