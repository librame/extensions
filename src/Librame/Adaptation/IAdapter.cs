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
using System.Reflection;

namespace Librame.Adaptation
{
    /// <summary>
    /// 适配器接口。
    /// </summary>
    public interface IAdapter : IDisposable
    {
        /// <summary>
        /// 获取适配器信息。
        /// </summary>
        AdapterInfo AdapterInfo { get; }

        /// <summary>
        /// 获取适配器配置目录。
        /// </summary>
        string ConfigDirectory { get; }


        /// <summary>
        /// 获取或设置管理器。
        /// </summary>
        IAdapterCollection Adapters { get; set; }

        /// <summary>
        /// 获取或设置首选项。
        /// </summary>
        AdapterSettings Settings { get; set; }


        /// <summary>
        /// 导出当前程序集包含的嵌入资源文件到适配器配置目录。
        /// </summary>
        /// <param name="outputRelativeFilePath">给定的输出相对文件路径（相对于 <see cref="ConfigDirectory"/> 适配器配置目录）。</param>
        /// <param name="manifestResourceName">给定的清单资源文件名（可选；默认以输出相对文件路径参考文件名）。</param>
        void ExportConfigDirectory(string outputRelativeFilePath, string manifestResourceName = null);
        /// <summary>
        /// 导出指定程序集包含的嵌入资源文件到适配器配置目录。
        /// </summary>
        /// <param name="adapterAssembly">给定包含嵌入资源文件的程序集。</param>
        /// <param name="outputRelativeFilePath">给定的输出相对文件路径（相对于 <see cref="ConfigDirectory"/> 适配器配置目录）。</param>
        /// <param name="manifestResourceName">给定的清单资源文件名（可选；默认以输出相对文件路径参考文件名）。</param>
        void ExportManifestResourceFile(Assembly adapterAssembly, string outputRelativeFilePath,
            string manifestResourceName = null);
    }
}
