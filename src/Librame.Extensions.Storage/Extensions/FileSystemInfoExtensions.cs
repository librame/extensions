#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.FileProviders.Physical;
using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Librame.Extensions.Storage
{
    /// <summary>
    /// 文件系统信息静态扩展。
    /// </summary>
    public static class FileSystemInfoExtensions
    {
        /// <summary>
        /// 是否被排除在外。
        /// </summary>
        /// <param name="fileSystemInfo">给定的 <see cref="FileSystemInfo"/>。</param>
        /// <param name="filters">给定的 <see cref="ExclusionFilters"/>。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "fileSystemInfo")]
        public static bool IsExcluded(this FileSystemInfo fileSystemInfo, ExclusionFilters filters)
        {
            if (filters == ExclusionFilters.None)
            {
                return false;
            }
            else if (fileSystemInfo.Name.StartsWith(".", StringComparison.Ordinal) && (filters & ExclusionFilters.DotPrefixed) != 0)
            {
                return true;
            }
            else if (fileSystemInfo.Exists &&
                (((fileSystemInfo.Attributes & FileAttributes.Hidden) != 0 && (filters & ExclusionFilters.Hidden) != 0) ||
                 ((fileSystemInfo.Attributes & FileAttributes.System) != 0 && (filters & ExclusionFilters.System) != 0)))
            {
                return true;
            }

            return false;
        }

    }
}
