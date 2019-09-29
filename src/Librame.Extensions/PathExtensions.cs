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
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace Librame.Extensions
{
    /// <summary>
    /// 路径静态扩展。
    /// </summary>
    public static class PathExtensions
    {
        private static readonly string _directorySeparator
            = Path.DirectorySeparatorChar.ToString(CultureInfo.CurrentCulture);

        private static readonly string _directoryEscapeSeparator
            = _directorySeparator == @"\" ? @"\\" : "/";

        //private static readonly string AltDirectorySeparator
        //= Path.AltDirectorySeparatorChar.ToString();

        private static readonly string _parentDirectorySeparatorKey
            = $"..{_directorySeparator}";

        private static readonly Regex _regexDevelopmentRelativePath
            = new Regex(@$"{_directoryEscapeSeparator}(bin|obj){_directoryEscapeSeparator}(Debug|Release)");


        /// <summary>
        /// 没有开发相对路径的目录。
        /// </summary>
        /// <param name="currentDirectory">给定的当前目录。</param>
        /// <returns>返回目录字符串。</returns>
        public static string WithoutDevelopmentRelativePath(this string currentDirectory)
        {
            currentDirectory.NotEmpty(nameof(currentDirectory));

            if (_regexDevelopmentRelativePath.IsMatch(currentDirectory))
            {
                var match = _regexDevelopmentRelativePath.Match(currentDirectory);
                return currentDirectory.Substring(0, match.Index);
            }

            return currentDirectory;
        }


        /// <summary>
        /// 子目录。
        /// </summary>
        /// <param name="directoryName">给定的目录名。</param>
        /// <param name="folderName">给定的目录名称。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo Subdirectory(this string directoryName, string folderName)
            => new DirectoryInfo(directoryName).Subdirectory(folderName);

        /// <summary>
        /// 子目录。
        /// </summary>
        /// <param name="directory">给定的 <see cref="DirectoryInfo"/>。</param>
        /// <param name="folderName">给定的目录名称。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo Subdirectory(this DirectoryInfo directory, string folderName)
        {
            directory.NotNull(nameof(directory));

            var subpath = directory.FullName.CombinePath(folderName);
            return new DirectoryInfo(subpath);
        }


        /// <summary>
        /// 改变指定路径的文件名。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <param name="newFileNameFactory">给定的新文件名方法。</param>
        /// <returns>返回路径。</returns>
        public static string ChangeFileName(this string path, Func<string, string, string> newFileNameFactory)
        {
            newFileNameFactory.NotNull(nameof(newFileNameFactory));

            var fileName = Path.GetFileName(path);
            var extension = Path.GetExtension(path);

            if (string.IsNullOrEmpty(fileName))
                return path;

            var index = fileName.LastIndexOf(extension, StringComparison.OrdinalIgnoreCase);
            var baseName = fileName.Substring(0, index);

            index = path.LastIndexOf(fileName, StringComparison.OrdinalIgnoreCase);
            var dir = path.Substring(0, index);

            fileName = newFileNameFactory.Invoke(baseName, extension);

            return Path.Combine(dir, fileName);
        }


        #region Combine


        /// <summary>
        /// 合并路径。
        /// </summary>
        /// <param name="basePath">给定的基础路径。</param>
        /// <param name="relativePath">给定要附加的相对路径。</param>
        /// <returns>返回路径字符串。</returns>
        public static string CombinePath(this string basePath, string relativePath)
        {
            basePath.NotEmpty(nameof(basePath));
            relativePath.NotEmpty(nameof(relativePath));

            // RelativePath: filename.ext
            if (!relativePath.Contains(_directorySeparator, StringComparison.OrdinalIgnoreCase))
                return Path.Combine(basePath, relativePath);

            // RelativePath: \filename.ext
            if (relativePath.StartsWith(_directorySeparator, StringComparison.OrdinalIgnoreCase))
                return Path.Combine(basePath, relativePath.TrimStart(_directorySeparator));

            // RelativePath: ..\filename.ext
            if (relativePath.StartsWith(_parentDirectorySeparatorKey, StringComparison.OrdinalIgnoreCase))
            {
                var baseInfo = new DirectoryInfo(basePath);
                var parent = BackParentPath(baseInfo, relativePath);
                return Path.Combine(parent.Info.FullName, parent.RelativePath);
            }

            return $"{basePath}{relativePath}";
        }

        private static (DirectoryInfo Info, string RelativePath) BackParentPath(DirectoryInfo info, string relativePath)
        {
            if (!relativePath.StartsWith(_parentDirectorySeparatorKey, StringComparison.OrdinalIgnoreCase))
                return (info, relativePath);

            // 链式返回（按层递进查找）
            return BackParentPath(info.Parent, relativePath.TrimStart(_parentDirectorySeparatorKey, false));
        }

        #endregion


        #region HasExtension

        /// <summary>
        /// 提取具有扩展名的路径集合。
        /// </summary>
        /// <param name="paths">给定的路径。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <returns>返回提取的路径集合。</returns>
        public static IEnumerable<string> ExtractHasExtension(this IEnumerable<string> paths, IEnumerable<string> extensions)
            => paths.ExtractHasExtension(extensions, (path, ext) => path.EndsWith(ext, StringComparison.OrdinalIgnoreCase));

        /// <summary>
        /// 提取具备某扩展名的元素集合。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="items">给定的元素集合。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <param name="hasExtensionFactory">给定具有扩展名的断定方法。</param>
        /// <returns>返回提取的集合。</returns>
        public static IEnumerable<T> ExtractHasExtension<T>(this IEnumerable<T> items, IEnumerable<string> extensions,
            Func<T, string, bool> hasExtensionFactory)
            => items.Where(item => item.TryHasExtension(extensions, hasExtensionFactory)).ToList();


        /// <summary>
        /// 尝试检测路径具备某扩展名。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryHasExtension(this string path, IEnumerable<string> extensions)
            => path.TryHasExtension(extensions, out string extension);

        /// <summary>
        /// 尝试检测路径具备某扩展名。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <param name="extension">输出具有的扩展名。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryHasExtension(this string path, IEnumerable<string> extensions, out string extension)
            => path.TryHasExtension(extensions, (p, ext) => p.EndsWith(ext, StringComparison.OrdinalIgnoreCase), out extension);


        /// <summary>
        /// 尝试检测元素具备某扩展名。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的元素。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <param name="hasExtensionFactory">给定具有扩展名的断定方法。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryHasExtension<T>(this T item, IEnumerable<string> extensions,
            Func<T, string, bool> hasExtensionFactory)
            => item.TryHasExtension(extensions, hasExtensionFactory, out _);

        /// <summary>
        /// 尝试检测元素具备某扩展名。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的元素。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <param name="hasExtensionFactory">给定具有扩展名的断定方法。</param>
        /// <param name="extension">输出具有的扩展名。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryHasExtension<T>(this T item, IEnumerable<string> extensions,
            Func<T, string, bool> hasExtensionFactory, out string extension)
        {
            extensions.NotNull(nameof(extensions));

            extension = string.Empty;
            foreach (var ext in extensions)
            {
                if (hasExtensionFactory.Invoke(item, ext))
                {
                    extension = ext;
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
