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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 扩展构建器依赖选项静态扩展。
    /// </summary>
    public static class ExtensionBuilderDependencyOptionsExtensions
    {
        /// <summary>
        /// 配置依赖选项。
        /// </summary>
        /// <typeparam name="TDependencyOptions">指定的依赖选项类型。</typeparam>
        /// <param name="setupAction">给定的依赖选项配置动作（可选）。</param>
        /// <returns>返回 <typeparamref name="TDependencyOptions"/>。</returns>
        public static TDependencyOptions ConfigureDependencyOptions<TDependencyOptions>(this Action<TDependencyOptions> setupAction)
            where TDependencyOptions : class, IExtensionBuilderDependencyOptions, new()
        {
            var dependencyOptions = new TDependencyOptions();
            setupAction?.Invoke(dependencyOptions);

            return dependencyOptions;
        }

    }
}
