#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象扩展构建器静态扩展。
    /// </summary>
    public static class AbstractionExtensionBuilderExtensions
    {
        /// <summary>
        /// 含有指定父级构建器类型的实例。
        /// </summary>
        /// <typeparam name="TParentBuilder">指定的父级构建器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <returns>返回是否成功获取的布尔值。</returns>
        public static bool HasParentBuilder<TParentBuilder>(this IExtensionBuilder builder)
            where TParentBuilder : IExtensionBuilder
        {
            return builder.TryGetParentBuilder<TParentBuilder>(out _);
        }

        /// <summary>
        /// 尝试获取指定父级构建器类型的实例。
        /// </summary>
        /// <typeparam name="TParentBuilder">指定的父级构建器类型。</typeparam>
        /// <param name="builder">给定的 <see cref="IExtensionBuilder"/>。</param>
        /// <param name="parentBuilder">输出父级 <see cref="IExtensionBuilder"/> 实例。</param>
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

    }
}
