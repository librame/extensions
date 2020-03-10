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

namespace Librame.Extensions.Core.Builders
{
    using Dependencies;

    /// <summary>
    /// 抽象扩展构建器依赖。
    /// </summary>
    /// <typeparam name="TOptions">指定的扩展构建器选项类型。</typeparam>
    public abstract class AbstractExtensionBuilderDependency<TOptions> : OptionsDependency<TOptions>, IExtensionBuilderDependency
        where TOptions : class, IExtensionBuilderOptions
    {
        private readonly static string _currentDirectory
            = Environment.CurrentDirectory.WithoutDevelopmentRelativePath();


        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilderDependency{TBuilderOptions}"/>。
        /// </summary>
        /// <param name="name">给定的依赖名称。</param>
        public AbstractExtensionBuilderDependency(string name)
            : base(name)
        {
        }


        /// <summary>
        /// 基础目录。
        /// </summary>
        public string BaseDirectory { get; set; }
            = _currentDirectory;

        /// <summary>
        /// 配置目录。
        /// </summary>
        public string ConfigDirectory { get; set; }
            = _currentDirectory;

        /// <summary>
        /// 导出目录。
        /// </summary>
        public string ExportDirectory { get; set; }
            = _currentDirectory;
    }
}
