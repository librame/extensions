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

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 文件系统接口。
    /// </summary>
    public interface IFileSystem
    {
        /// <summary>
        /// 名称。
        /// </summary>
        string Name { get; }

        /// <summary>
        /// 路径。
        /// </summary>
        string Path { get; }

        /// <summary>
        /// 创建时间。
        /// </summary>
        DateTime CreateTime { get; }

        /// <summary>
        /// 更新时间。
        /// </summary>
        DateTime UpdateTime { get; }

        /// <summary>
        /// 大小。
        /// </summary>
        long Size { get; }

        /// <summary>
        /// 格式化大小。
        /// </summary>
        string FormatSize { get; }
    }
}
