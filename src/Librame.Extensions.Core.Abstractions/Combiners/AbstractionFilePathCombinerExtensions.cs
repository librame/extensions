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

namespace Librame.Extensions.Core.Combiners
{
    /// <summary>
    /// 抽象文件路径组合器静态扩展。
    /// </summary>
    public static class AbstractionFilePathCombinerExtensions
    {
        /// <summary>
        /// 转换为文件路径组合器。
        /// </summary>
        /// <param name="fileName">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public static FilePathCombiner AsFilePathCombiner(this FileNameCombiner fileName, string basePath = null)
        {
            var combiner = new FilePathCombiner(fileName);

            if (!basePath.IsEmpty())
                combiner.ChangeBasePath(basePath);

            return combiner;
        }

        /// <summary>
        /// 转换为文件路径组合器。
        /// </summary>
        /// <param name="filePath">给定的文件路径。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        public static FilePathCombiner AsFilePathCombiner(this string filePath, string basePath = null)
        {
            var combiner = new FilePathCombiner(filePath);

            if (!basePath.IsEmpty())
                combiner.ChangeBasePath(basePath);

            return combiner;
        }

        /// <summary>
        /// 将文件名数组转换为文件路径组合器数组。
        /// </summary>
        /// <param name="filePaths">给定的文件路径数组。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/> 数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "filePaths")]
        public static FilePathCombiner[] AsFilePathCombiners(this string[] filePaths, string basePath = null)
        {
            filePaths.NotEmpty(nameof(filePaths));

            var combiners = new FilePathCombiner[filePaths.Length];

            for (var i = 0; i < filePaths.Length; i++)
                combiners[i] = filePaths[i].AsFilePathCombiner(basePath);

            return combiners;
        }


        /// <summary>
        /// 当作数组。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/> 集合。</returns>
        public static FilePathCombiner[] AsArray(this FilePathCombiner combiner)
        {
            combiner.NotNull(nameof(combiner));
            return new FilePathCombiner[] { combiner };
        }

        /// <summary>
        /// 转换为字符串集合。
        /// </summary>
        /// <param name="combiners">给定的 <see cref="FilePathCombiner"/> 集合。</param>
        /// <returns>返回字符串集合。</returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<FilePathCombiner> combiners)
            => combiners.NotEmpty(nameof(combiners)).Select(combiner => combiner?.ToString());


        /// <summary>
        /// 批量改变基础路径。
        /// </summary>
        /// <param name="combiners">给定的 <see cref="FilePathCombiner"/> 集合。</param>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/> 集合。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "combiners")]
        public static IEnumerable<FilePathCombiner> ChangeBasePath(this IEnumerable<FilePathCombiner> combiners, string newBasePath)
        {
            combiners.NotEmpty(nameof(combiners));

            foreach (var combiner in combiners)
                combiner?.ChangeBasePath(newBasePath);

            return combiners;
        }

        /// <summary>
        /// 改变基础路径。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="newBasePathFactory">给定的新基础路径工厂方法（输入参数为当前基础路径）。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "combiner")]
        public static FilePathCombiner ChangeBasePath(this FilePathCombiner combiner, Func<string, string> newBasePathFactory)
            => combiner.NotNull(nameof(combiner)).ChangeBasePath(newBasePathFactory?.Invoke(combiner.BasePath));

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "combiner")]
        public static FilePathCombiner ChangeFileName(this FilePathCombiner combiner, Func<FileNameCombiner, string> newFileNameFactory)
            => combiner.NotNull(nameof(combiner)).ChangeFileName(newFileNameFactory?.Invoke(combiner.FileName));

        /// <summary>
        /// 改变文件名。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "combiner")]
        public static FilePathCombiner ChangeFileName(this FilePathCombiner combiner, Func<FileNameCombiner, FileNameCombiner> newFileNameFactory)
            => combiner.NotNull(nameof(combiner)).ChangeFileName(newFileNameFactory?.Invoke(combiner.FileName));


        /// <summary>
        /// 依据当前文件组合器的文件名与指定的基础路径，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="newBasePathFactory">给定的新基础路径工厂方法（输入参数为当前基础路径）。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "combiner")]
        public static FilePathCombiner NewBasePath(this FilePathCombiner combiner, Func<string, string> newBasePathFactory)
            => combiner.NotNull(nameof(combiner)).NewBasePath(newBasePathFactory?.Invoke(combiner.BasePath));

        /// <summary>
        /// 依据当前文件组合器的基础路径与指定的文件名，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "combiner")]
        public static FilePathCombiner NewFileName(this FilePathCombiner combiner, Func<FileNameCombiner, string> newFileNameFactory)
            => combiner.NotNull(nameof(combiner)).NewFileName(newFileNameFactory?.Invoke(combiner.FileName));

        /// <summary>
        /// 依据当前文件组合器的基础路径与指定的文件名，新建一个 <see cref="FilePathCombiner"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法。</param>
        /// <returns>返回 <see cref="FilePathCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "combiner")]
        public static FilePathCombiner NewFileName(this FilePathCombiner combiner, Func<FileNameCombiner, FileNameCombiner> newFileNameFactory)
            => combiner.NotNull(nameof(combiner)).NewFileName(newFileNameFactory?.Invoke(combiner.FileName));


        /// <summary>
        /// 转换为 <see cref="FileInfo"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回 <see cref="FileInfo"/>。</returns>
        public static FileInfo AsFileInfo(this FilePathCombiner combiner)
            => new FileInfo(combiner?.ToString());

        /// <summary>
        /// 将基础路径转换为 <see cref="DirectoryInfo"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo AsDirectoryInfo(this FilePathCombiner combiner)
            => new DirectoryInfo(combiner?.BasePath);

        /// <summary>
        /// 创建基础路径的目录。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回 <see cref="DirectoryInfo"/>。</returns>
        public static DirectoryInfo CreateDirectory(this FilePathCombiner combiner)
        {
            var directoryInfo = combiner.AsDirectoryInfo();
            directoryInfo.Create();

            return directoryInfo;
        }


        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        public static void Delete(this FilePathCombiner combiner)
            => File.Delete(combiner?.ToString());

        /// <summary>
        /// 删除文件。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="fileInfo">给定的 <see cref="FileInfo"/>。</param>
        public static void Delete(this FilePathCombiner combiner, out FileInfo fileInfo)
        {
            fileInfo = combiner.AsFileInfo();
            fileInfo.Delete();
        }


        /// <summary>
        /// 文件是否存在。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists(this FilePathCombiner combiner)
            => File.Exists(combiner?.ToString());

        /// <summary>
        /// 文件是否存在。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="fileInfo">给定的 <see cref="FileInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool Exists(this FilePathCombiner combiner, out FileInfo fileInfo)
        {
            fileInfo = combiner.AsFileInfo();
            return fileInfo.Exists;
        }


        /// <summary>
        /// 目录是否存在。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        public static void DirectoryExists(this FilePathCombiner combiner)
            => Directory.Delete(combiner?.ToString());

        /// <summary>
        /// 目录是否存在。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FilePathCombiner"/>。</param>
        /// <param name="directoryInfo">给定的 <see cref="DirectoryInfo"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool DirectoryExists(this FilePathCombiner combiner, out DirectoryInfo directoryInfo)
        {
            directoryInfo = combiner.AsDirectoryInfo();
            return directoryInfo.Exists;
        }

    }
}
