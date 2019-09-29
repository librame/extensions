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
    /// 抽象扩展构建器依赖选项。
    /// </summary>
    /// <typeparam name="TBuilderOptions">指定的构建器选项类型。</typeparam>
    public abstract class AbstractExtensionBuilderDependencyOptions<TBuilderOptions> : AbstractOptionsConfigurator, IExtensionBuilderDependencyOptions
        where TBuilderOptions : class, IExtensionBuilderOptions
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilderDependencyOptions{TBuilderOptions}"/>。
        /// </summary>
        /// <param name="name">给定的名称（可选）。</param>
        public AbstractExtensionBuilderDependencyOptions(string name = null)
            : base(name)
        {
        }


        /// <summary>
        /// 基础目录。
        /// </summary>
        public string BaseDirectory { get; set; }
            = Environment.CurrentDirectory.WithoutDevelopmentRelativePath();

        /// <summary>
        /// 构建器选项配置器。
        /// </summary>
        public OptionsActionConfigurator<TBuilderOptions> Builder { get; }
            = new OptionsActionConfigurator<TBuilderOptions>();
    }
}
