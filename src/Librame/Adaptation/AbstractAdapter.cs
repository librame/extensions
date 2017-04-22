#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Adaptation
{
    using Utility;

    /// <summary>
    /// 抽象适配器。
    /// </summary>
    public abstract class AbstractAdapter : KeyBuilder, IAdapter
    {
        /// <summary>
        /// 获取适配器信息。
        /// </summary>
        public abstract AdapterInfo AdapterInfo { get; }

        /// <summary>
        /// 获取适配器配置目录。
        /// </summary>
        public virtual string AdapterConfigDirectory
        {
            get { return PathUtility.ConfigsDirectory.AppendDirectoryName(AdapterInfo.Name); }
        }


        /// <summary>
        /// 获取或设置管理器。
        /// </summary>
        public IAdapterManager Adapters { get; set; }

        /// <summary>
        /// 获取或设置首选项。
        /// </summary>
        public AdapterSettings Settings { get; set; }


        /// <summary>
        /// 将资源路径转换为嵌入的清单资源名。
        /// </summary>
        /// <param name="resourcePath">给定的资源路径。</param>
        /// <returns>返回清单资源名。</returns>
        public virtual string ToManifestResourceName(string resourcePath)
        {
            resourcePath.GuardNullOrEmpty(nameof(resourcePath));

            var manifestResourceName = resourcePath;

            if (manifestResourceName.Contains("\\"))
                manifestResourceName = manifestResourceName.Replace('\\', '.');

            if (manifestResourceName.Contains("/"))
                manifestResourceName = manifestResourceName.Replace('/', '.');

            if (!manifestResourceName.StartsWith(LibrameAssemblyConstants.NAME))
                manifestResourceName = (LibrameAssemblyConstants.NAME + "." + manifestResourceName);

            return manifestResourceName;
        }

        /// <summary>
        /// 导出当前适配器嵌入的资源配置文件
        /// </summary>
        /// <param name="outputRelativeFilePath">给定的输出相对文件路径（相对于 <see cref="AdapterConfigDirectory"/> 适配器配置目录）。</param>
        /// <param name="manifestResourceName">给定的清单资源名。</param>
        public virtual void ExportConfigFile(string outputRelativeFilePath, string manifestResourceName)
        {
            var outputFilePath = outputRelativeFilePath;

            // 如果不是以适配器配置目录开始的，则添加配置目录
            if (!outputFilePath.StartsWith(AdapterConfigDirectory))
                outputFilePath = AdapterConfigDirectory.AppendPath(outputFilePath);

            // 导出嵌入的资源配置文件
            AssemblyUtility.ExportManifestResourceFile(outputFilePath, manifestResourceName);
        }


        /// <summary>
        /// 释放适配器资源。
        /// </summary>
        public virtual void Dispose()
        {
            // None.
        }

    }
}
