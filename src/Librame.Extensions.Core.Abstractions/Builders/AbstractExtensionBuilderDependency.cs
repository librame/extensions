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
        /// 构造一个 <see cref="AbstractExtensionBuilderDependency{TOptions}"/>。
        /// </summary>
        /// <param name="name">给定的依赖名称。</param>
        /// <param name="parentDependency">给定的父级 <see cref="IExtensionBuilderDependency"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
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

                ParentDependency = parentDependency;
            }
            else
            {
                var currentDirectory = Environment.CurrentDirectory.WithoutDevelopmentRelativePath();

                BaseDirectory = currentDirectory;
                ConfigDirectory = currentDirectory.CombinePath(CoreSettings.Preference.ConfigsFolder);
                ReportDirectory = currentDirectory.CombinePath(CoreSettings.Preference.ReportsFolder);
                ResourceDirectory = currentDirectory.CombinePath(CoreSettings.Preference.ResourcesFolder);
            }
        }


        /// <summary>
        /// 父级依赖。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilderDependency"/>。
        /// </value>
        [Newtonsoft.Json.JsonIgnore]
        [System.Text.Json.Serialization.JsonIgnore]
        public IExtensionBuilderDependency ParentDependency { get; }

        /// <summary>
        /// 基础目录（通常为根目录）。
        /// </summary>
        public string BaseDirectory { get; set; }

        /// <summary>
        /// 配置目录（用于存储功能配置的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        public string ConfigDirectory { get; set; }

        /// <summary>
        /// 报告目录（用于存储动态生成信息的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        public string ReportDirectory { get; set; }

        /// <summary>
        /// 资源目录（用于存储静态资源的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        public string ResourceDirectory { get; set; }
    }
}
