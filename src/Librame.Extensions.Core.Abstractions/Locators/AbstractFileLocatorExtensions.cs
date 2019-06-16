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
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象文件定位器静态扩展。
    /// </summary>
    public static class AbstractFileLocatorExtensions
    {
        /// <summary>
        /// 当作数组。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <returns>返回 <see cref="IFileLocator"/> 集合。</returns>
        public static IFileLocator[] AsArray(this IFileLocator locator)
        {
            return new IFileLocator[] { locator };
        }

        /// <summary>
        /// 转换为字符串集合。
        /// </summary>
        /// <param name="locators">给定的 <see cref="IFileLocator"/> 集合。</param>
        /// <returns>返回字符串集合。</returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<IFileLocator> locators)
        {
            return locators.Select(locator => locator?.ToString());
        }


        /// <summary>
        /// 批量改变基础路径。
        /// </summary>
        /// <param name="locators">给定的 <see cref="IFileLocator"/> 集合。</param>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/> 集合。</returns>
        public static IEnumerable<IFileLocator> ChangeBasePath(this IEnumerable<IFileLocator> locators, string newBasePath)
        {
            locators.NotNull(nameof(locators));

            foreach (var locator in locators)
                locator?.ChangeBasePath(newBasePath);

            return locators;
        }

        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="newBasePathFactory">给定的新基础路径工厂方法（输入参数为当前基础路径）。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator ChangeBasePath(this IFileLocator locator, Func<string, string> newBasePathFactory)
        {
            locator.NotNull(nameof(locator));

            return locator.ChangeBasePath(newBasePathFactory?.Invoke(locator.BasePath));
        }

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator ChangeFileName(this IFileLocator locator, Func<IFileNameLocator, string> newFileNameFactory)
        {
            locator.NotNull(nameof(locator));

            return locator.ChangeFileName(newFileNameFactory?.Invoke(locator.FileName));
        }
        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator ChangeFileName(this IFileLocator locator, Func<IFileNameLocator, IFileNameLocator> newFileNameFactory)
        {
            locator.NotNull(nameof(locator));

            return locator.ChangeFileName(newFileNameFactory?.Invoke(locator.FileName));
        }


        /// <summary>
        /// 依据当前文件定位器的文件名与指定的基础路径，新建一个 <see cref="IFileLocator"/> 实例。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="newBasePathFactory">给定的新基础路径工厂方法（输入参数为当前基础路径）。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator NewBasePath(this IFileLocator locator, Func<string, string> newBasePathFactory)
        {
            locator.NotNull(nameof(locator));

            return locator.NewBasePath(newBasePathFactory?.Invoke(locator.BasePath));
        }

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个 <see cref="IFileLocator"/> 实例。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator NewFileName(this IFileLocator locator, Func<IFileNameLocator, string> newFileNameFactory)
        {
            locator.NotNull(nameof(locator));

            return locator.NewFileName(newFileNameFactory?.Invoke(locator.FileName));
        }
        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个 <see cref="IFileLocator"/> 实例。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator NewFileName(this IFileLocator locator, Func<IFileNameLocator, IFileNameLocator> newFileNameFactory)
        {
            locator.NotNull(nameof(locator));

            return locator.NewFileName(newFileNameFactory?.Invoke(locator.FileName));
        }


        /// <summary>
        /// 转换为 <see cref="FileInfo"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <returns>返回 <see cref="FileInfo"/>。</returns>
        public static FileInfo AsFileInfo(this IFileLocator locator)
        {
            return new FileInfo(locator?.ToString());
        }

        /// <summary>
        /// 将基础路径转换为 <see cref="DirectoryInfo"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo AsDirectoryInfo(this IFileLocator locator)
        {
            return new DirectoryInfo(locator?.BasePath);
        }

        /// <summary>
        /// 创建基础路径的目录。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo CreateDirectory(this IFileLocator locator)
        {
            var directoryInfo = locator.AsDirectoryInfo();
            directoryInfo.Create();

            return directoryInfo;
        }


        /// <summary>
        /// 文件是否存在。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists(this IFileLocator locator)
        {
            return locator.Exists(out _);
        }
        /// <summary>
        /// 文件是否存在。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="fileInfo">给定的 <see cref="FileInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists(this IFileLocator locator, out FileInfo fileInfo)
        {
            fileInfo = locator.AsFileInfo();

            return fileInfo.Exists;
        }


        /// <summary>
        /// 目录是否存在。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool DirectoryExists(this IFileLocator locator)
        {
            return locator.DirectoryExists(out _);
        }
        /// <summary>
        /// 目录是否存在。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="directoryInfo">给定的 <see cref="DirectoryInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool DirectoryExists(this IFileLocator locator, out DirectoryInfo directoryInfo)
        {
            directoryInfo = locator.AsDirectoryInfo();

            return directoryInfo.Exists;
        }

    }
}
