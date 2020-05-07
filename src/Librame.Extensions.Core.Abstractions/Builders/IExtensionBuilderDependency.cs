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
        /// 基础目录。
        /// </summary>
        string BaseDirectory { get; set; }

        /// <summary>
        /// 配置目录。
        /// </summary>
        string ConfigDirectory { get; set; }

        /// <summary>
        /// 报告目录。
        /// </summary>
        string ReportDirectory { get; set; }

        /// <summary>
        /// 资源目录。
        /// </summary>
        string ResourceDirectory { get; set; }
    }
}
