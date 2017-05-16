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
using System.Runtime.InteropServices;

namespace Librame.Resource
{
    /// <summary>
    /// 资源来源信息。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
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
    }
}
