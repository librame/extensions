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
        string AdapterConfigDirectory { get; }


        /// <summary>
        /// 获取或设置管理器。
        /// </summary>
        IAdapterManager Adapters { get; set; }

        /// <summary>
        /// 获取或设置首选项。
        /// </summary>
        AdapterSettings Settings { get; set; }


        /// <summary>
        /// 将资源路径转换为嵌入的清单资源名。
        /// </summary>
        /// <param name="resourcePath">给定的资源路径。</param>
        /// <returns>返回清单资源名。</returns>
        string ToManifestResourceName(string resourcePath);

        /// <summary>
        /// 导出当前适配器嵌入的资源配置文件
        /// </summary>
        /// <param name="outputRelativeFilePath">给定的输出相对文件路径（相对于 <see cref="AdapterConfigDirectory"/> 适配器配置目录）。</param>
        /// <param name="manifestResourceName">给定的清单资源名。</param>
        void ExportConfigFile(string outputRelativeFilePath, string manifestResourceName);
    }
}
