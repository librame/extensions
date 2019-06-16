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

namespace Librame.Extensions
{
    /// <summary>
    /// 路径静态扩展。
    /// </summary>
    public static class PathExtensions
    {
        /// <summary>
        /// 转换为文件信息。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="FileInfo"/>。</returns>
        public static FileInfo AsFileInfo(this string fileName)
        {
            return new FileInfo(fileName);
        }

        /// <summary>
        /// 转换为目录信息。
        /// </summary>
        /// <param name="directoryName">给定的目录名。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo AsDirectoryInfo(this string directoryName)
        {
            return new DirectoryInfo(directoryName);
        }

        /// <summary>
        /// 子目录。
        /// </summary>
        /// <param name="directory">给定的 <see cref="DirectoryInfo"/>。</param>
        /// <param name="folderName">给定的目录名称。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo Subdirectory(this DirectoryInfo directory, string folderName)
        {
            var subpath = directory.FullName.CombinePath(folderName);

            return subpath.AsDirectoryInfo();
        }


        /// <summary>
        /// 改变指定路径的文件名。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <param name="newFileNameFactory">给定的新文件名方法。</param>
        /// <returns>返回路径。</returns>
        public static string ChangeFileName(this string path, Func<string, string, string> newFileNameFactory)
        {
            var fileName = Path.GetFileName(path);
            var extension = Path.GetExtension(path);

            if (string.IsNullOrEmpty(fileName))
                return path;

            var index = fileName.LastIndexOf(extension);
            var baseName = fileName.Substring(0, index);

            index = path.LastIndexOf(fileName);
            var dir = path.Substring(0, index);

            fileName = newFileNameFactory?.Invoke(baseName, extension);

            return Path.Combine(dir, fileName);
        }


        #region Combine

        /// <summary>
        /// 附加到路径或 URI。
        /// </summary>
        /// <param name="basePathOrUri">给定的基础路径或 URI。</param>
        /// <param name="relativeOrAbsolutePath">给定要附加的相对或绝对路径（绝对路径仅支持 URI）。</param>
        /// <returns>返回路径字符串。</returns>
        public static string AppendPathOrUri(this string basePathOrUri, string relativeOrAbsolutePath)
        {
            if (basePathOrUri.IsNullOrEmpty()) return relativeOrAbsolutePath;

            if (basePathOrUri.Contains(Uri.SchemeDelimiter))
                return CombineUriToString(basePathOrUri, relativeOrAbsolutePath);

            return CombinePath(basePathOrUri, relativeOrAbsolutePath);
        }


        private static readonly string DirectorySeparator = Path.DirectorySeparatorChar.ToString();
        private static readonly string AltDirectorySeparator = Path.AltDirectorySeparatorChar.ToString();
        private static readonly string ParentDirectorySeparatorKey = $"..{DirectorySeparator}";

        /// <summary>
        /// 合并路径。
        /// </summary>
        /// <param name="basePath">给定的基础路径。</param>
        /// <param name="relativePath">给定要附加的相对路径。</param>
        /// <returns>返回路径字符串。</returns>
        public static string CombinePath(this string basePath, string relativePath)
        {
            if (relativePath.IsNullOrEmpty()) return basePath;

            // RelativePath: filename.ext
            if (!relativePath.Contains(DirectorySeparator))
                return Path.Combine(basePath, relativePath);

            // RelativePath: \filename.ext
            if (relativePath.StartsWith(DirectorySeparator))
                return Path.Combine(basePath, relativePath.TrimStart(DirectorySeparator));

            // RelativePath: ..\filename.ext
            if (relativePath.StartsWith(ParentDirectorySeparatorKey))
            {
                var baseInfo = new DirectoryInfo(basePath);
                var parent = BackParentPath(baseInfo, relativePath);
                return Path.Combine(parent.Info.FullName, parent.RelativePath);
            }

            return $"{basePath}{relativePath}";
        }
        private static (DirectoryInfo Info, string RelativePath) BackParentPath(DirectoryInfo info, string relativePath)
        {
            if (!relativePath.StartsWith(ParentDirectorySeparatorKey))
                return (info, relativePath);

            // 链式返回（按层递进查找）
            return BackParentPath(info.Parent, relativePath.TrimStart(ParentDirectorySeparatorKey, false));
        }


        /// <summary>
        /// 合并 URI。
        /// </summary>
        /// <param name="baseUri">给定的基础 URI。</param>
        /// <param name="relativeOrAbsoluteUri">给定的相对或绝对虚拟路径。</param>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        public static Uri CombineUri(this Uri baseUri, string relativeOrAbsoluteUri)
        {
            return new Uri(baseUri, relativeOrAbsoluteUri);
        }
        /// <summary>
        /// 合并 URI 字符串。
        /// </summary>
        /// <param name="baseUri">给定的基础 URI。</param>
        /// <param name="relativeOrAbsoluteUri">给定的相对或绝对虚拟路径。</param>
        /// <returns>返回字符串。</returns>
        public static string CombineUriToString(this Uri baseUri, string relativeOrAbsoluteUri)
        {
            return CombineUri(baseUri, relativeOrAbsoluteUri).ToString();
        }

        /// <summary>
        /// 合并 URI。
        /// </summary>
        /// <param name="baseUri">给定的基础 URI 字符串。</param>
        /// <param name="relativeOrAbsoluteUri">给定的相对或绝对虚拟路径。</param>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        public static Uri CombineUri(this string baseUri, string relativeOrAbsoluteUri)
        {
            return CombineUri(new Uri(baseUri), relativeOrAbsoluteUri);
        }
        /// <summary>
        /// 合并 URI 字符串。
        /// </summary>
        /// <remarks>
        /// 样例1：
        /// <code>
        /// var baseUri = "http://www.domain.name/";
        /// var relativeOrAbsoluteUri = "controller/action";
        /// // http://www.domain.name/controller/action
        /// return baseUri.CombineUriToString(relativeOrAbsoluteUri);
        /// </code>
        /// 样例2：
        /// <code>
        /// var baseUri = "http://www.domain.name/webapi/entities";
        /// var relativeOrAbsoluteUri = "/controller/action";
        /// // http://www.domain.name/controller/action
        /// return baseUri.CombineUriToString(relativeOrAbsoluteUri);
        /// </code>
        /// </remarks>
        /// <param name="baseUri">给定的基础 URI 字符串。</param>
        /// <param name="relativeOrAbsoluteUri">给定的相对或绝对虚拟路径。</param>
        /// <returns>返回字符串。</returns>
        public static string CombineUriToString(this string baseUri, string relativeOrAbsoluteUri)
        {
            return CombineUri(baseUri, relativeOrAbsoluteUri).ToString();
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
        {
            return paths.ExtractHasExtension(extensions, (path, ext) => path.EndsWith(ext));
        }

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
        {
            return items.Where(item => item.TryHasExtension(extensions, hasExtensionFactory)).ToList();
        }


        /// <summary>
        /// 尝试检测路径具备某扩展名。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryHasExtension(this string path, IEnumerable<string> extensions)
        {
            return path.TryHasExtension(extensions, out string extension);
        }

        /// <summary>
        /// 尝试检测路径具备某扩展名。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <param name="extension">输出具有的扩展名。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryHasExtension(this string path, IEnumerable<string> extensions, out string extension)
        {
            return path.TryHasExtension(extensions, (p, ext) => p.EndsWith(ext), out extension);
        }


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
        {
            return item.TryHasExtension(extensions, hasExtensionFactory, out _);
        }

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
