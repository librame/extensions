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
using System.Diagnostics.CodeAnalysis;

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
        /// <summary>
        /// 构造一个 <see cref="AbstractExtensionBuilderDependency{TBuilderOptions}"/>。
        /// </summary>
        /// <param name="name">给定的依赖名称。</param>
        /// <param name="parentDependency">给定的父级 <see cref="IExtensionBuilderDependency"/>（可选）。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "parentDependency")]
        protected AbstractExtensionBuilderDependency(string name, IExtensionBuilderDependency parentDependency = null)
            : base(name)
        {
            if (parentDependency.IsNotNull())
            {
                BaseDirectory = parentDependency.BaseDirectory;
                ConfigDirectory = parentDependency.ConfigDirectory;
                ReportDirectory = parentDependency.ReportDirectory;
                ResourceDirectory = parentDependency.ResourceDirectory;

                ConfigurationRoot = parentDependency.ConfigurationRoot;

                if (ConfigurationRoot.IsNotNull())
                    Configuration = ConfigurationRoot.GetSection(Name);
            }
            else
            {
                var currentDirectory = Environment.CurrentDirectory.WithoutDevelopmentRelativePath();
                BaseDirectory = currentDirectory;
                ConfigDirectory = currentDirectory.CombinePath("configs");
                ReportDirectory = currentDirectory.CombinePath("reports");
                ResourceDirectory = currentDirectory.CombinePath("resources");
            }
        }


        /// <summary>
        /// 基础目录。
        /// </summary>
        public string BaseDirectory { get; set; }

        /// <summary>
        /// 配置目录。
        /// </summary>
        public string ConfigDirectory { get; set; }

        /// <summary>
        /// 报告目录。
        /// </summary>
        public string ReportDirectory { get; set; }

        /// <summary>
        /// 资源目录。
        /// </summary>
        public string ResourceDirectory { get; set; }
    }
}
