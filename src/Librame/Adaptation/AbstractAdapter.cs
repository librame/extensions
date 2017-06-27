#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Reflection;

namespace Librame.Adaptation
{
    using Utility;

    /// <summary>
    /// 抽象适配器。
    /// </summary>
    public abstract class AbstractAdapter : KeyBuilder, IAdapter
    {
        /// <summary>
        /// 获取当前适配器信息。
        /// </summary>
        public abstract AdapterInfo AdapterInfo { get; }

        /// <summary>
        /// 获取当前适配器配置目录。
        /// </summary>
        public virtual string ConfigDirectory
        {
            get { return PathUtility.ConfigsDirectory.AppendPath(AdapterInfo.Name); }
        }


        /// <summary>
        /// 获取或设置管理器。
        /// </summary>
        public IAdapterCollection Adapters { get; set; }

        /// <summary>
        /// 获取或设置首选项。
        /// </summary>
        public AdapterSettings Settings { get; set; }


        /// <summary>
        /// 导出当前程序集包含的嵌入资源文件到适配器配置目录。
        /// </summary>
        /// <param name="outputRelativeFilePath">给定的输出相对文件路径（相对于 <see cref="ConfigDirectory"/> 适配器配置目录）。</param>
        /// <param name="manifestResourceName">给定的清单资源文件名（可选；默认以输出相对文件路径参考文件名）。</param>
        public virtual void ExportConfigDirectory(string outputRelativeFilePath, string manifestResourceName = null)
        {
            ExportManifestResourceFile(AssemblyUtility.CurrentAssembly, outputRelativeFilePath, manifestResourceName);
        }
        /// <summary>
        /// 导出指定程序集包含的嵌入资源文件到适配器配置目录。
        /// </summary>
        /// <param name="adapterAssembly">给定包含嵌入资源文件的程序集。</param>
        /// <param name="outputRelativeFilePath">给定的输出相对文件路径（相对于 <see cref="ConfigDirectory"/> 适配器配置目录）。</param>
        /// <param name="manifestResourceName">给定的清单资源文件名（可选；默认以输出相对文件路径参考文件名）。</param>
        public virtual void ExportManifestResourceFile(Assembly adapterAssembly, string outputRelativeFilePath,
            string manifestResourceName = null)
        {
            outputRelativeFilePath.NotEmpty(nameof(outputRelativeFilePath));
            
            // 如果不是以适配器配置目录开始的，则添加配置目录
            if (!outputRelativeFilePath.StartsWith(ConfigDirectory))
            {
                // 支持基础目录
                if (outputRelativeFilePath.StartsWith(PathUtility.BaseDirectory))
                    outputRelativeFilePath = PathUtility.BaseDirectory.AppendPath(outputRelativeFilePath);
                else
                    outputRelativeFilePath = ConfigDirectory.AppendPath(outputRelativeFilePath);
            }

            if (string.IsNullOrEmpty(manifestResourceName))
                manifestResourceName = ToManifestResourceName(adapterAssembly, outputRelativeFilePath);

            // 尝试创建配置目录
            System.IO.Directory.CreateDirectory(ConfigDirectory);

            // 导出嵌入的资源配置文件
            adapterAssembly.ManifestResourceSaveAs(manifestResourceName, outputRelativeFilePath);
        }

        /// <summary>
        /// 将资源路径转换为嵌入的清单资源名。
        /// </summary>
        /// <param name="adapterAssembly">给定包含嵌入资源文件的程序集。</param>
        /// <param name="resourceFilePath">给定的资源文件路径。</param>
        /// <returns>返回清单资源名。</returns>
        protected virtual string ToManifestResourceName(Assembly adapterAssembly, string resourceFilePath)
        {
            adapterAssembly.NotNull(nameof(adapterAssembly));

            var assemblyName = new AssemblyName(adapterAssembly.FullName);

            // 移除基础目录
            var manifestResourceName = resourceFilePath.Replace(PathUtility.BaseDirectory, string.Empty);

            if (manifestResourceName.Contains("\\"))
                manifestResourceName = manifestResourceName.Replace('\\', '.');

            if (manifestResourceName.Contains("/"))
                manifestResourceName = manifestResourceName.Replace('/', '.');

            if (manifestResourceName.StartsWith("."))
                manifestResourceName = manifestResourceName.TrimStart('.');

            // 附加命令空间
            if (!manifestResourceName.StartsWith(assemblyName.Name))
                manifestResourceName = (assemblyName.Name + "." + manifestResourceName);

            return manifestResourceName;
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
