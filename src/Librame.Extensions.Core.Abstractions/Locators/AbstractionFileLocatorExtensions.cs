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
    public static class AbstractionFileLocatorExtensions
    {
        /// <summary>
        /// 转换为文件定位器。
        /// </summary>
        /// <param name="fileName">给定的 <see cref="FileNameLocator"/>。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public static FileLocator AsFileLocator(this FileNameLocator fileName, string basePath = null)
        {
            var locator = new FileLocator(fileName);

            if (!basePath.IsNullOrEmpty())
                locator.ChangeBasePath(basePath);

            return locator;
        }

        /// <summary>
        /// 转换为文件定位器。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public static FileLocator AsFileLocator(this string fileName, string basePath = null)
        {
            var locator = (FileLocator)fileName;

            if (!basePath.IsNullOrEmpty())
                locator.ChangeBasePath(basePath);

            return locator;
        }

        /// <summary>
        /// 将文件名数组转换为文件定位器数组。
        /// </summary>
        /// <param name="fileNames">给定的文件名数组。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="FileLocator"/> 数组。</returns>
        public static FileLocator[] AsFileLocators(this string[] fileNames, string basePath = null)
        {
            var locators = new FileLocator[fileNames.Length];

            for (var i = 0; i < fileNames.Length; i++)
                locators[i] = fileNames[i].AsFileLocator(basePath);

            return locators;
        }


        /// <summary>
        /// 当作数组。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <returns>返回 <see cref="FileLocator"/> 集合。</returns>
        public static FileLocator[] AsArray(this FileLocator locator)
        {
            locator.NotNull(nameof(locator));
            return new FileLocator[] { locator };
        }

        /// <summary>
        /// 转换为字符串集合。
        /// </summary>
        /// <param name="locators">给定的 <see cref="FileLocator"/> 集合。</param>
        /// <returns>返回字符串集合。</returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<FileLocator> locators)
            => locators.NotNullOrEmpty(nameof(locators))
            .Select(locator => locator?.ToString());


        /// <summary>
        /// 批量改变基础路径。
        /// </summary>
        /// <param name="locators">给定的 <see cref="FileLocator"/> 集合。</param>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="FileLocator"/> 集合。</returns>
        public static IEnumerable<FileLocator> ChangeBasePath(this IEnumerable<FileLocator> locators, string newBasePath)
        {
            locators.NotNullOrEmpty(nameof(locators));

            foreach (var locator in locators)
                locator?.ChangeBasePath(newBasePath);

            return locators;
        }

        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="newBasePathFactory">给定的新基础路径工厂方法（输入参数为当前基础路径）。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public static FileLocator ChangeBasePath(this FileLocator locator, Func<string, string> newBasePathFactory)
            => locator.NotNull(nameof(locator))
            .ChangeBasePath(newBasePathFactory?.Invoke(locator.BasePath));

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public static FileLocator ChangeFileName(this FileLocator locator, Func<FileNameLocator, string> newFileNameFactory)
            => locator.NotNull(nameof(locator))
            .ChangeFileName(newFileNameFactory?.Invoke(locator.FileName));

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public static FileLocator ChangeFileName(this FileLocator locator, Func<FileNameLocator, FileNameLocator> newFileNameFactory)
            => locator.NotNull(nameof(locator))
            .ChangeFileName(newFileNameFactory?.Invoke(locator.FileName));


        /// <summary>
        /// 依据当前文件定位器的文件名与指定的基础路径，新建一个 <see cref="FileLocator"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="newBasePathFactory">给定的新基础路径工厂方法（输入参数为当前基础路径）。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public static FileLocator NewBasePath(this FileLocator locator, Func<string, string> newBasePathFactory)
            => locator.NotNull(nameof(locator))
            .NewBasePath(newBasePathFactory?.Invoke(locator.BasePath));

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个 <see cref="FileLocator"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public static FileLocator NewFileName(this FileLocator locator, Func<FileNameLocator, string> newFileNameFactory)
            => locator.NotNull(nameof(locator))
            .NewFileName(newFileNameFactory?.Invoke(locator.FileName));

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个 <see cref="FileLocator"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="FileLocator"/>。</returns>
        public static FileLocator NewFileName(this FileLocator locator, Func<FileNameLocator, FileNameLocator> newFileNameFactory)
            => locator.NotNull(nameof(locator))
            .NewFileName(newFileNameFactory?.Invoke(locator.FileName));


        /// <summary>
        /// 转换为 <see cref="FileInfo"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <returns>返回 <see cref="FileInfo"/>。</returns>
        public static FileInfo AsFileInfo(this FileLocator locator)
            => new FileInfo(locator?.ToString());

        /// <summary>
        /// 将基础路径转换为 <see cref="DirectoryInfo"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo AsDirectoryInfo(this FileLocator locator)
            => new DirectoryInfo(locator?.BasePath);

        /// <summary>
        /// 创建基础路径的目录。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo CreateDirectory(this FileLocator locator)
        {
            var directoryInfo = locator.AsDirectoryInfo();
            directoryInfo.Create();

            return directoryInfo;
        }


        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        public static void Delete(this FileLocator locator)
            => File.Delete(locator?.ToString());

        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="fileInfo">给定的 <see cref="FileInfo"/>。</param>
        public static void Delete(this FileLocator locator, out FileInfo fileInfo)
        {
            fileInfo = locator.AsFileInfo();
            fileInfo.Delete();
        }


        /// <summary>
        /// 文件是否存在。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists(this FileLocator locator)
            => File.Exists(locator?.ToString());

        /// <summary>
        /// 文件是否存在。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="fileInfo">给定的 <see cref="FileInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists(this FileLocator locator, out FileInfo fileInfo)
        {
            fileInfo = locator.AsFileInfo();
            return fileInfo.Exists;
        }


        /// <summary>
        /// 目录是否存在。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        public static void DirectoryExists(this FileLocator locator)
            => Directory.Delete(locator?.ToString());

        /// <summary>
        /// 目录是否存在。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileLocator"/>。</param>
        /// <param name="directoryInfo">给定的 <see cref="DirectoryInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool DirectoryExists(this FileLocator locator, out DirectoryInfo directoryInfo)
        {
            directoryInfo = locator.AsDirectoryInfo();
            return directoryInfo.Exists;
        }

    }
}
