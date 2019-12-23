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
        /// 基础目录。
        /// </summary>
        string BaseDirectory { get; set; }

        /// <summary>
        /// 配置目录。
        /// </summary>
        string ConfigDirectory { get; set; }

        /// <summary>
        /// 导出目录。
        /// </summary>
        string ExportDirectory { get; set; }
    }
}
