#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core.Builders
{
    using Dependencies;

    /// <summary>
    /// 扩展构建器依赖接口。
    /// </summary>
    public interface IExtensionBuilderDependency : IDependency
    {
        /// <summary>
        /// 父级构建器依赖。
        /// </summary>
        /// <value>
        /// 返回 <see cref="IExtensionBuilderDependency"/>。
        /// </value>
        IExtensionBuilderDependency ParentDependency { get; }

        /// <summary>
        /// 基础目录（通常为根目录）。
        /// </summary>
        string BaseDirectory { get; set; }

        /// <summary>
        /// 配置目录（用于存储功能配置的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        string ConfigDirectory { get; set; }

        /// <summary>
        /// 报告目录（用于存储动态生成信息的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        string ReportDirectory { get; set; }

        /// <summary>
        /// 资源目录（用于存储静态资源的文件夹，通常为一级目录；子级目录可以此为基础路径与模块名+功能目录名为相对路径进行组合）。
        /// </summary>
        string ResourceDirectory { get; set; }
    }
}
