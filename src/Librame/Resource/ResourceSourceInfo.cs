﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Resource
{
    using Utility;

    /// <summary>
    /// 资源来源信息。
    /// </summary>
    public class ResourceSourceInfo
    {
        /// <summary>
        /// 资源结构类型字符串形式。
        /// </summary>
        public string SchemaTypeString { get; set; }

        /// <summary>
        /// 资源路径（支持本地或远程格式）。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 资源另存为文件名（仅支持本地路径）。
        /// </summary>
        public string SaveAsFilename { get; set; }

        /// <summary>
        /// 启用监视（不支持 URL 路径资源）。
        /// </summary>
        public bool EnableWatching { get; set; }


        /// <summary>
        /// 创建资源来源信息。
        /// </summary>
        /// <typeparam name="TSchema">指定的资源结构类型。</typeparam>
        /// <param name="path">给定的资源路径（支持本地路径或远程链接）。</param>
        /// <param name="saveAsFilename">给定的另存为文件名（可选）。</param>
        /// <param name="enableWatching">是否启用监控文件变化。</param>
        /// <returns>返回 <see cref="ResourceSourceInfo"/>。</returns>
        public static ResourceSourceInfo CreateInfo<TSchema>(string path, string saveAsFilename = null, bool enableWatching = true)
        {
            return CreateInfo(typeof(TSchema), path, saveAsFilename, enableWatching);
        }
        /// <summary>
        /// 创建资源来源信息。
        /// </summary>
        /// <param name="schemaType">给定的资源结构类型。</param>
        /// <param name="path">给定的资源路径（支持本地路径或远程链接）。</param>
        /// <param name="saveAsFilename">给定的另存为文件名（可选；默认等于本地资源路径）。</param>
        /// <param name="enableWatching">是否启用监控文件变化。</param>
        /// <returns>返回 <see cref="ResourceSourceInfo"/>。</returns>
        public static ResourceSourceInfo CreateInfo(Type schemaType, string path, string saveAsFilename = null, bool enableWatching = true)
        {
            path.NotNullOrEmpty(nameof(path));

            var configDirectory = LibrameArchitecture.AdapterManager.Resource.AdapterConfigDirectory;
            if (!System.IO.Path.IsPathRooted(path))
                path = configDirectory.AppendPath(path);

            return new ResourceSourceInfo()
            {
                SchemaTypeString = TypeUtility.AssemblyQualifiedNameWithoutVCP(schemaType),
                Path = path,
                SaveAsFilename = saveAsFilename ?? path,
                EnableWatching = enableWatching
            };
        }

    }
}
