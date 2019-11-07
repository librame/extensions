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
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Librame.Extensions
{
    using Resources;

    /// <summary>
    /// 路径静态扩展。
    /// </summary>
    /// <remarks>
    /// 一、路径分隔符：
    /// 1. Windows 系统支持使用正斜杠'/' (由 <see cref="Path.AltDirectorySeparatorChar"/> 字段返回) 
    /// 或反斜杠'\' (<see cref="Path.DirectorySeparatorChar"/> 由字段返回) 作为路径分隔符。
    /// 2. Unix 系统只支持正斜杠'/'。
    /// 二、路径常量参考：
    /// Path.AltDirectorySeparatorChar: '/'。
    /// Path.DirectorySeparatorChar: '/'。
    /// Path.PathSeparator: ':'。
    /// Path.VolumeSeparatorChar: '/'。
    /// Path.GetInvalidPathChars: U+007C),U+0000,...。
    /// https://docs.microsoft.com/zh-cn/dotnet/api/system.io.path.directoryseparatorchar?view=netframework-4.8。
    /// </remarks>
    public static class PathExtensions
    {
        /// <summary>
        /// 没有开发相对路径的路径。
        /// </summary>
        /// <param name="currentPath">给定的当前目录。</param>
        /// <returns>返回目录字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "currentPath")]
        public static string WithoutDevelopmentRelativePath(this string currentPath)
        {
            currentPath.NotEmpty(nameof(currentPath));

            var regex = new Regex(GetPattern());
            if (regex.IsMatch(currentPath))
            {
                var match = regex.Match(currentPath);
                return currentPath.Substring(0, match.Index);
            }

            return currentPath;

            string GetPattern()
            {
                var separator = currentPath.CompatibleContains(ExtensionSettings.DirectorySeparatorChar)
                    ? Regex.Escape(ExtensionSettings.DirectorySeparator)
                    : ExtensionSettings.AltDirectorySeparator;

                var sb = new StringBuilder();
                sb.Append($"({separator}bin|{separator}obj)");
                sb.Append($"({separator}x86|{separator}x64)?");
                sb.Append($"({separator}Debug|{separator}Release)");

                return sb.ToString();
            }
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "directory")]
        public static DirectoryInfo Subdirectory(this DirectoryInfo directory, string folderName)
        {
            directory.NotNull(nameof(directory));

            var subpath = directory.FullName.CombinePath(folderName);
            return new DirectoryInfo(subpath);
        }


        /// <summary>
        /// 改变指定路径的文件名。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Invalid file path '{0}'.
        /// </exception>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="newFileNameFactory">给定的新文件名方法；输入参数依次为文件基础名、文件扩展名（不存在则为 <see cref="string.Empty"/>）。</param>
        /// <returns>返回路径。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "newFileNameFactory")]
        public static string ChangeFileName(this string filePath, Func<string, string, string> newFileNameFactory)
        {
            newFileNameFactory.NotNull(nameof(newFileNameFactory));

            var fileName = Path.GetFileName(filePath);
            if (string.IsNullOrEmpty(fileName))
                throw new ArgumentException(InternalResource.ArgumentExceptionFilePathFormat.Format(filePath));

            var basePath = Path.GetDirectoryName(filePath);
            var extension = Path.GetExtension(filePath);

            // 文件扩展名可能为空
            if (extension.IsEmpty())
                return basePath.CombinePath(newFileNameFactory.Invoke(fileName, string.Empty));

            var baseName = fileName.TrimEnd(extension, loops: false);
            return basePath.CombinePath(newFileNameFactory.Invoke(baseName, extension));
        }


        #region CombinePath

        /// <summary>
        /// 合并路径。
        /// </summary>
        /// <exception cref="ArgumentException">
        /// Invalid base path '{0}'.
        /// </exception>
        /// <param name="basePath">给定的基础路径。</param>
        /// <param name="relativePath">给定要附加的相对路径。</param>
        /// <returns>返回路径字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "basePath")]
        public static string CombinePath(this string basePath, string relativePath)
        {
            basePath.NotEmpty(nameof(basePath));
            relativePath.NotEmpty(nameof(relativePath));

            var altSeparatorIndex = basePath.CompatibleIndexOf(ExtensionSettings.AltDirectorySeparatorChar);
            var separatorIndex = basePath.CompatibleIndexOf(ExtensionSettings.DirectorySeparatorChar);

            // 正反向分隔符不能同时存在（或不存在）于基础路径中，且不能位于开头字符
            if ((altSeparatorIndex < 1 && separatorIndex < 1) || (altSeparatorIndex > 0 && separatorIndex > 0))
                throw new ArgumentException(InternalResource.ArgumentExceptionBasePathFormat.Format(basePath));

            var basePathSeparator = altSeparatorIndex > 0 ? ExtensionSettings.AltDirectorySeparatorChar : ExtensionSettings.DirectorySeparatorChar;

            // RelativePath: filename.ext
            if (!relativePath.CompatibleContains(ExtensionSettings.AltDirectorySeparatorChar)
                && !relativePath.CompatibleContains(ExtensionSettings.DirectorySeparatorChar))
                return $"{basePath.EnsureTrailing(basePathSeparator)}{relativePath}";

            // RelativePath: ../ or ..\
            if (basePath.TryCombineParentPath(relativePath, basePathSeparator, out string resultPath))
                return resultPath;

            // RelativePath: ./ or .\, / or \
            if (basePath.TryCombineSiblingPath(relativePath, basePathSeparator, out resultPath))
                return resultPath;

            return $"{basePath.EnsureTrailing(basePathSeparator)}{relativePath}";
        }

        private static bool TryCombineParentPath(this string basePath, string relativePath, char basePathSeparator, out string resultPath)
        {
            var parentMark = "..";

            // RelativePath: ../
            var altSeparatorMark = $"{parentMark}{ExtensionSettings.AltDirectorySeparatorChar}";
            if (relativePath.StartsWith(altSeparatorMark, StringComparison.OrdinalIgnoreCase))
            {
                var parent = BackToParentPath(new DirectoryInfo(basePath), relativePath, altSeparatorMark);
                resultPath = $"{parent.Info.FullName.EnsureTrailing(basePathSeparator)}{parent.RelativePath}";
                return true;
            }

            // RelativePath: ..\
            var separatorMark = $"{parentMark}{ExtensionSettings.DirectorySeparatorChar}";
            if (relativePath.StartsWith(separatorMark, StringComparison.OrdinalIgnoreCase))
            {
                var parent = BackToParentPath(new DirectoryInfo(basePath), relativePath, separatorMark);
                resultPath = $"{parent.Info.FullName.EnsureTrailing(basePathSeparator)}{parent.RelativePath}";
                return true;
            }

            resultPath = null;
            return false;

            // BackToParentPath
            (DirectoryInfo Info, string RelativePath) BackToParentPath(DirectoryInfo info, string relativePath, string currentSeparatorMark)
            {
                if (!relativePath.StartsWith(currentSeparatorMark, StringComparison.OrdinalIgnoreCase))
                    return (info, relativePath);

                // 链式返回（按层递进查找）
                return BackToParentPath(info.Parent, relativePath.TrimStart(currentSeparatorMark, loops: false), currentSeparatorMark);
            }
        }

        private static bool TryCombineSiblingPath(this string basePath, string relativePath, char basePathSeparator, out string resultPath)
        {
            var siblingMark = ".";

            // RelativePath: ./
            var altSeparatorMark = $"{siblingMark}{ExtensionSettings.AltDirectorySeparatorChar}";
            if (relativePath.StartsWith(altSeparatorMark, StringComparison.OrdinalIgnoreCase))
            {
                resultPath = $"{basePath.EnsureTrailing(basePathSeparator)}{relativePath.TrimStart(altSeparatorMark)}";
                return true;
            }

            // RelativePath: .\
            var separatorMark = $"{siblingMark}{ExtensionSettings.DirectorySeparatorChar}";
            if (relativePath.StartsWith(separatorMark, StringComparison.OrdinalIgnoreCase))
            {
                resultPath = $"{basePath.EnsureTrailing(basePathSeparator)}{relativePath.TrimStart(separatorMark)}";
                return true;
            }

            // RelativePath: /
            if (relativePath.CompatibleStartsWith(ExtensionSettings.AltDirectorySeparatorChar))
            {
                resultPath = $"{basePath.EnsureTrailing(basePathSeparator)}{relativePath.TrimStart(ExtensionSettings.AltDirectorySeparatorChar)}";
                return true;
            }

            // RelativePath: \
            if (relativePath.CompatibleStartsWith(ExtensionSettings.DirectorySeparatorChar))
            {
                resultPath = $"{basePath.EnsureTrailing(basePathSeparator)}{relativePath.TrimStart(ExtensionSettings.DirectorySeparatorChar)}";
                return true;
            }

            resultPath = null;
            return false;
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
            => path.TryHasExtension(extensions, out _);

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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "extensions")]
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
