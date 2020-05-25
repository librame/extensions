#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;

namespace Librame.Extensions.Core.Combiners
{
    /// <summary>
    /// <see cref="FileNameCombiner"/> 静态扩展。
    /// </summary>
    public static class AbstractionFileNameCombinerExtensions
    {
        /// <summary>
        /// 转换为文件名组合器。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public static FileNameCombiner AsFileNameCombiner(this string fileName)
            => new FileNameCombiner(fileName);

        /// <summary>
        /// 转换为文件名组合器。
        /// </summary>
        /// <param name="baseName">给定的基础名。</param>
        /// <param name="extension">给定的扩展名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/>。</returns>
        public static FileNameCombiner AsFileNameCombiner(this string baseName, string extension)
            => new FileNameCombiner(baseName, extension);

        /// <summary>
        /// 将文件名数组转换为文件名组合器数组。
        /// </summary>
        /// <param name="fileNames">给定的文件名数组。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/> 数组。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static FileNameCombiner[] AsFileNameCombiners(this string[] fileNames)
        {
            fileNames.NotEmpty(nameof(fileNames));

            var combiners = new FileNameCombiner[fileNames.Length];

            for (var i = 0; i < fileNames.Length; i++)
                combiners[i] = new FileNameCombiner(fileNames[i]);

            return combiners;
        }


        /// <summary>
        /// 当作数组。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="FileNameCombiner"/>。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/> 集合。</returns>
        public static FileNameCombiner[] AsArray(this FileNameCombiner combiner)
        {
            combiner.NotNull(nameof(combiner));
            return new FileNameCombiner[] { combiner };
        }

        /// <summary>
        /// 转换为字符串集合。
        /// </summary>
        /// <param name="combiners">给定的 <see cref="FileNameCombiner"/> 集合。</param>
        /// <returns>返回字符串集合。</returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<FileNameCombiner> combiners)
        {
            combiners.NotEmpty(nameof(combiners));
            return combiners.Select(combiner => combiner?.ToString());
        }


        /// <summary>
        /// 批量改变扩展名。
        /// </summary>
        /// <param name="combiners">给定的 <see cref="FileNameCombiner"/> 集合。</param>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="FileNameCombiner"/> 集合。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static IEnumerable<FileNameCombiner> ChangeExtension(this IEnumerable<FileNameCombiner> combiners, string newExtension)
        {
            combiners.NotNull(nameof(combiners));

            foreach (var combiner in combiners)
                combiner?.ChangeExtension(newExtension);

            return combiners;
        }

    }
}
