#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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
        /// 清除无效的文件名字符集合。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数", Justification = "<挂起>")]
        public static string ClearInvalidFileNameChars(this string fileName)
        {
            fileName.NotEmpty(nameof(fileName));

            foreach (var ch in Path.GetInvalidFileNameChars())
            {
                fileName = fileName.CompatibleReplace(ch.ToString(), string.Empty);
            }

            return fileName;
        }


        /// <summary>
        /// 不带开发相对路径的路径。
        /// </summary>
        /// <param name="currentPath">给定的当前目录。</param>
        /// <returns>返回目录字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
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
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
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
        /// <param name="newFileNameFactory">给定的新文件名方法；输入参数依次为文件基础名、文件扩展名（可空）。</param>
        /// <returns>返回路径。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string ChangeFileName(this string filePath, Func<string, string, string> newFileNameFactory)
        {
            newFileNameFactory.NotNull(nameof(newFileNameFactory));

            (string baseName, string extension) = filePath.GetFileBaseNameAndExtension(out var basePath);

            var newFileName = newFileNameFactory.Invoke(baseName, extension);

            if (basePath.IsEmpty())
                return newFileName;

            return basePath.CombinePath(newFileName);
        }

        /// <summary>
        /// 获取文件基础名与扩展名的元组。
        /// </summary>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="basePath">输出基础路径（可空）。</param>
        /// <returns>返回一个包含基础名与扩展名（可空）的元组。</returns>
        public static (string baseName, string extension) GetFileBaseNameAndExtension
            (this string filePath, out string basePath)
        {
            var fileName = filePath.GetFileNameWithoutPath(out basePath);
            if (fileName.IsEmpty())
                throw new ArgumentException(InternalResource.ArgumentExceptionFilePathFormat.Format(filePath));

            var index = fileName.LastIndexOf('.');

            return index switch
            {
                -1 => (fileName, string.Empty),
                0 => (string.Empty, fileName),
                _ => (fileName.Substring(0, index), fileName.Substring(index)) // 保留分隔符
            };
        }

        /// <summary>
        /// 获取不包含路径的文件名。
        /// </summary>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="basePath">输出基础路径（可空）。</param>
        /// <returns>返回字符串。</returns>
        [SuppressMessage("Design", "CA1062:验证公共方法的参数")]
        public static string GetFileNameWithoutPath(this string filePath, out string basePath)
        {
            filePath.NotEmpty(nameof(filePath));

            var altSeparatorIndex = filePath.LastIndexOf(ExtensionSettings.AltDirectorySeparatorChar);
            var separatorIndex = filePath.LastIndexOf(ExtensionSettings.DirectorySeparatorChar);

            // 如果基础路径无分隔符
            if (altSeparatorIndex < 0 && separatorIndex < 0)
            {
                basePath = null;
                return filePath;
            }

            var index = separatorIndex >= 0
                ? separatorIndex
                : altSeparatorIndex;

            basePath = filePath.Substring(0, index + 1); // 基础路径保留分隔符
            return filePath.Substring(index + 1);
        }


        #region CombinePath

        /// <summary>
        /// 合并到当前目录。
        /// </summary>
        /// <param name="relativePath">给定要附加的相对路径。</param>
        /// <param name="withoutDevelopmentRelativePath">不带开发相对路径的路径（可选；默认不带）。</param>
        /// <returns>返回路径字符串。</returns>
        public static string CombineCurrentDirectory(this string relativePath,
            bool withoutDevelopmentRelativePath = true)
        {
            var directory = Directory.GetCurrentDirectory();

            if (withoutDevelopmentRelativePath)
                directory = directory.WithoutDevelopmentRelativePath();

            return directory.CombinePath(relativePath);
        }

        /// <summary>
        /// 合并路径。
        /// </summary>
        /// <param name="basePath">给定的基础路径。</param>
        /// <param name="relativePath">给定要附加的相对路径。</param>
        /// <returns>返回路径字符串。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static string CombinePath(this string basePath, string relativePath)
        {
            basePath.NotEmpty(nameof(basePath));
            relativePath.NotEmpty(nameof(relativePath));

            var altSeparatorIndex = basePath.CompatibleIndexOf(ExtensionSettings.AltDirectorySeparatorChar);
            var separatorIndex = basePath.CompatibleIndexOf(ExtensionSettings.DirectorySeparatorChar);

            //// 正反向分隔符不能同时存在（或不存在）于基础路径中
            //if (altSeparatorIndex >= 0 && separatorIndex >= 0 || altSeparatorIndex < 0 && separatorIndex < 0)
            //    throw new ArgumentException(InternalResource.ArgumentExceptionBasePathFormat.Format(basePath));

            // 如果基础路径无分隔符
            if (altSeparatorIndex < 0 && separatorIndex < 0)
                return $"{basePath}{relativePath}";

            // 如果基础路径同时包含两种分隔符
            if (altSeparatorIndex >= 0 && separatorIndex >= 0)
            {
                // 则以最靠前的分隔符为基准，替换掉后面的其他分隔符
                if (altSeparatorIndex < separatorIndex)
                {
                    basePath = basePath.Replace(ExtensionSettings.DirectorySeparatorChar,
                        ExtensionSettings.AltDirectorySeparatorChar);
                    //separatorIndex = -1; // 冗余
                }
                else
                {
                    basePath = basePath.Replace(ExtensionSettings.AltDirectorySeparatorChar,
                        ExtensionSettings.DirectorySeparatorChar);
                    altSeparatorIndex = -1;
                }
            }

            var basePathSeparator = altSeparatorIndex >= 0
                ? ExtensionSettings.AltDirectorySeparatorChar
                : ExtensionSettings.DirectorySeparatorChar;

            return CombineRelativePath(basePath, relativePath, basePathSeparator);
        }

        private static string CombineRelativePath(this string basePath, string relativePath, char basePathSeparator)
        {
            var reverseSeparator = basePathSeparator == ExtensionSettings.AltDirectorySeparatorChar
                ? ExtensionSettings.DirectorySeparatorChar
                : ExtensionSettings.AltDirectorySeparatorChar;

            // 尝试替换相对路径中的反向分隔符为基础路径分隔符
            if (relativePath.CompatibleContains(reverseSeparator))
                relativePath = relativePath.Replace(reverseSeparator, basePathSeparator);

            // RelativePath: ../ or ..\
            var parentSeparator = $"..{basePathSeparator}";
            if (relativePath.StartsWith(parentSeparator, StringComparison.OrdinalIgnoreCase))
            {
                var (info, _relativePath) = BackToParentPath(new DirectoryInfo(basePath), relativePath, parentSeparator);
                return $"{info.FullName.EnsureTrailing(basePathSeparator)}{_relativePath}";
            }

            // RelativePath: ./ or .\
            var siblingSeparator = $".{basePathSeparator}";
            if (relativePath.StartsWith(siblingSeparator, StringComparison.OrdinalIgnoreCase))
                return $"{basePath.EnsureTrailing(basePathSeparator)}{relativePath.TrimStart(siblingSeparator)}";

            // RelativePath: / or \
            if (relativePath.CompatibleStartsWith(basePathSeparator))
                return $"{basePath.EnsureTrailing(basePathSeparator)}{relativePath.TrimStart(basePathSeparator)}";

            return $"{basePath.EnsureTrailing(basePathSeparator)}{relativePath}";

            // BackToParentPath
            (DirectoryInfo info, string relativePath) BackToParentPath(DirectoryInfo info, string relativePath, string parentSeparator)
            {
                if (!relativePath.StartsWith(parentSeparator, StringComparison.OrdinalIgnoreCase))
                    return (info, relativePath);

                // 链式返回（按层递进查找）
                return BackToParentPath(info.Parent, relativePath.TrimStart(parentSeparator, loops: false), parentSeparator);
            }
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
        /// <param name="validFactory">给定的验证工厂方法。</param>
        /// <returns>返回提取的集合。</returns>
        public static IEnumerable<T> ExtractHasExtension<T>(this IEnumerable<T> items, IEnumerable<string> extensions,
            Func<T, string, bool> validFactory)
            => items.Where(item => item.TryGetExtension(extensions, validFactory)).ToList();


        /// <summary>
        /// 尝试获取路径中包含的扩展名。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGetExtension(this string path, IEnumerable<string> extensions)
            => path.TryGetExtension(extensions, out _);

        /// <summary>
        /// 尝试获取路径中包含的扩展名。
        /// </summary>
        /// <param name="path">给定的路径。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <param name="result">输出具有的扩展名。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGetExtension(this string path, IEnumerable<string> extensions, out string result)
            => path.TryGetExtension(extensions, (p, ext) => p.EndsWith(ext, StringComparison.OrdinalIgnoreCase), out result);


        /// <summary>
        /// 尝试获取与验证工厂方法匹配的扩展名。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的元素。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <param name="validFactory">给定的验证工厂方法。</param>
        /// <returns>返回布尔值。</returns>
        public static bool TryGetExtension<T>(this T item, IEnumerable<string> extensions,
            Func<T, string, bool> validFactory)
            => item.TryGetExtension(extensions, validFactory, out _);

        /// <summary>
        /// 尝试获取与验证工厂方法匹配的扩展名。
        /// </summary>
        /// <typeparam name="T">指定的类型。</typeparam>
        /// <param name="item">给定的元素。</param>
        /// <param name="extensions">给定的扩展名集合。</param>
        /// <param name="validFactory">给定的验证工厂方法。</param>
        /// <param name="result">输出扩展名。</param>
        /// <returns>返回布尔值。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static bool TryGetExtension<T>(this T item, IEnumerable<string> extensions,
            Func<T, string, bool> validFactory, out string result)
        {
            extensions.NotNull(nameof(extensions));
            validFactory.NotNull(nameof(validFactory));

            result = string.Empty;
            foreach (var extension in extensions)
            {
                if (validFactory.Invoke(item, extension))
                {
                    result = extension;
                    return true;
                }
            }

            return false;
        }

        #endregion

    }
}
