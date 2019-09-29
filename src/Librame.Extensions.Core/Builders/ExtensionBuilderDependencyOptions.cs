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
    /// 扩展构建器依赖选项。
    /// </summary>
    /// <typeparam name="TDependencyOptions">指定的构建器依赖选项类型。</typeparam>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public class ExtensionBuilderDependencyOptions<TDependencyOptions, TBuilderOptions> : AbstractExtensionBuilderDependencyOptions<TBuilderOptions>
        where TDependencyOptions : class, IExtensionBuilderDependencyOptions
        where TBuilderOptions : class, IExtensionBuilderOptions
    {
        /// <summary>
        /// 构造一个 <see cref="ExtensionBuilderDependencyOptions{TDependencyOptions, TBuilderOptions}"/>。
        /// </summary>
        public ExtensionBuilderDependencyOptions()
            : this(GetOptionsName<TDependencyOptions>())
        {
        }

        /// <summary>
        /// 构造一个 <see cref="ExtensionBuilderDependencyOptions{TDependencyOptions, TBuilderOptions}"/>。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        protected ExtensionBuilderDependencyOptions(string name)
            : base(name)
        {
        }


        /// <summary>
        /// 选项类型。
        /// </summary>
        public override Type OptionsType { get; }
            = typeof(TDependencyOptions);
    }
}
