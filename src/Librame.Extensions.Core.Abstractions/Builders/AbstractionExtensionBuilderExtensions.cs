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
    /// <see cref="IExtensionBuilder"/> 静态扩展。
    /// </summary>
    public static class AbstractionExtensionBuilderExtensions
    {
        /// <summary>
        /// 获取必需的构建器实例。
        /// </summary>
        /// <typeparam name="TBuilder">指定的构建器类型。</typeparam>
        /// <param name="currentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="excludeCurrentBuilder">排除当前 <see cref="IExtensionBuilder"/>（可选；默认不排除）。</param>
        /// <returns>返回 <typeparamref name="TBuilder"/> 或抛出 <see cref="InvalidOperationException"/>。</returns>
        public static TBuilder GetRequiredBuilder<TBuilder>(this IExtensionBuilder currentBuilder,
            bool excludeCurrentBuilder = false)
            where TBuilder : class, IExtensionBuilder
        {
            if (!currentBuilder.TryGetBuilder<TBuilder>(out var resultBuilder, excludeCurrentBuilder))
                throw new InvalidOperationException($"The current builder is not or does not contain a parent builder '{typeof(TBuilder)}'.");

            return resultBuilder;
        }

        /// <summary>
        /// 包含指定构建器类型的实例（支持查找当前或父级实例）。
        /// </summary>
        /// <typeparam name="TBuilder">指定的构建器类型。</typeparam>
        /// <param name="currentBuilder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="excludeCurrentBuilder">排除当前 <see cref="IExtensionBuilder"/>（可选；默认不排除）。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool ContainsBuilder<TBuilder>(this IExtensionBuilder currentBuilder,
            bool excludeCurrentBuilder = false)
            where TBuilder : class, IExtensionBuilder
            => currentBuilder.TryGetBuilder<TBuilder>(out _, excludeCurrentBuilder);

        /// <summary>
        /// 尝试获取指定构建器类型的实例（支持查找当前或父级实例）。
        /// </summary>
        /// <typeparam name="TBuilder">指定的构建器类型。</typeparam>
        /// <param name="currentBuilder">给定的当前 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="resultBuilder">输出结果 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="excludeCurrentBuilder">排除当前 <see cref="IExtensionBuilder"/>（可选；默认不排除）。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool TryGetBuilder<TBuilder>(this IExtensionBuilder currentBuilder,
            out TBuilder resultBuilder, bool excludeCurrentBuilder = false)
            where TBuilder : class, IExtensionBuilder
        {
            resultBuilder = LookupBuilder(excludeCurrentBuilder
                ? currentBuilder?.ParentBuilder
                : currentBuilder);
            
            return resultBuilder.IsNotNull();

            // LookupBuilder
            TBuilder LookupBuilder(IExtensionBuilder extensionBuilder)
            {
                if (extensionBuilder.IsNull())
                    return null;

                if (extensionBuilder is TBuilder builder)
                    return builder;

                // 链式查找父级构建器
                return LookupBuilder(extensionBuilder.ParentBuilder);
            }
        }


        /// <summary>
        /// 枚举当前以及父级构建器字典集合。
        /// </summary>
        /// <param name="currentBuilder">给定的当前 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="excludeCurrentBuilder">排除当前 <see cref="IExtensionBuilder"/>（可选；默认不排除）。</param>
        /// <returns>返回字典集合。</returns>
        public static Dictionary<string, IExtensionBuilder> EnumerateBuilders
            (this IExtensionBuilder currentBuilder, bool excludeCurrentBuilder = false)
        {
            var builders = new Dictionary<string, IExtensionBuilder>();

            LookupBuilder(excludeCurrentBuilder
                ? currentBuilder?.ParentBuilder
                : currentBuilder);

            return builders;

            // LookupBuilder
            void LookupBuilder(IExtensionBuilder builder)
            {
                if (builder.IsNull())
                    return;

                builders.Add(builder.Name, builder);

                // 链式查找父级构建器
                LookupBuilder(builder.ParentBuilder);
            }
        }

    }
}
