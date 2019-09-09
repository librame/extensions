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
using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象文件名定位器静态扩展。
    /// </summary>
    public static class AbstractionFileNameLocatorExtensions
    {
        /// <summary>
        /// 转换为文件名定位器。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        public static FileNameLocator AsFileNameLocator(this string fileName)
        {
            return (FileNameLocator)fileName;
        }

        /// <summary>
        /// 转换为文件名定位器。
        /// </summary>
        /// <param name="baseName">给定的基础名。</param>
        /// <param name="extension">给定的扩展名。</param>
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        public static FileNameLocator AsFileNameLocator(this string baseName, string extension)
        {
            return new FileNameLocator(baseName, extension);
        }

        /// <summary>
        /// 将文件名数组转换为文件名定位器数组。
        /// </summary>
        /// <param name="fileNames">给定的文件名数组。</param>
        /// <returns>返回 <see cref="FileNameLocator"/> 数组。</returns>
        public static FileNameLocator[] AsFileNameLocators(this string[] fileNames)
        {
            var locators = new FileNameLocator[fileNames.Length];

            for (var i = 0; i < fileNames.Length; i++)
                locators[i] = (FileNameLocator)fileNames[i];

            return locators;
        }


        /// <summary>
        /// 当作数组。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileNameLocator"/>。</param>
        /// <returns>返回 <see cref="FileNameLocator"/> 集合。</returns>
        public static FileNameLocator[] AsArray(this FileNameLocator locator)
        {
            locator.NotNull(nameof(locator));
            return new FileNameLocator[] { locator };
        }

        /// <summary>
        /// 转换为字符串集合。
        /// </summary>
        /// <param name="locators">给定的 <see cref="FileNameLocator"/> 集合。</param>
        /// <returns>返回字符串集合。</returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<FileNameLocator> locators)
        {
            locators.NotNullOrEmpty(nameof(locators));
            return locators.Select(locator => locator?.ToString());
        }


        /// <summary>
        /// 改变基础名。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileNameLocator"/>。</param>
        /// <param name="newBaseNameFactory">给定的新基础名工厂方法（输入参数为当前基础名）。</param>
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        public static FileNameLocator ChangeBaseName(this FileNameLocator locator, Func<string, string> newBaseNameFactory)
            => locator.NotNull(nameof(locator))
            .ChangeBaseName(newBaseNameFactory?.Invoke(locator.BaseName));

        /// <summary>
        /// 改变扩展名。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileNameLocator"/>。</param>
        /// <param name="newExtensionFactory">给定的新扩展名工厂方法（输入参数为当前扩展名）。</param>
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        public static FileNameLocator ChangeExtension(this FileNameLocator locator, Func<string, string> newExtensionFactory)
            => locator.NotNull(nameof(locator))
            .ChangeExtension(newExtensionFactory?.Invoke(locator.Extension));

        /// <summary>
        /// 批量改变扩展名。
        /// </summary>
        /// <param name="locators">给定的 <see cref="FileNameLocator"/> 集合。</param>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="FileNameLocator"/> 集合。</returns>
        public static IEnumerable<FileNameLocator> ChangeExtension(this IEnumerable<FileNameLocator> locators, string newExtension)
        {
            locators.NotNull(nameof(locators));

            foreach (var locator in locators)
                locator?.ChangeExtension(newExtension);

            return locators;
        }


        /// <summary>
        /// 依据当前文件定位器的扩展名与指定的基础名，新建一个 <see cref="FileNameLocator"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileNameLocator"/>。</param>
        /// <param name="newBaseNameFactory">给定的新基础名工厂方法（输入参数为当前基础名）。</param>
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        public static FileNameLocator NewBaseName(this FileNameLocator locator, Func<string, string> newBaseNameFactory)
            => locator.NotNull(nameof(locator))
            .NewBaseName(newBaseNameFactory?.Invoke(locator.BaseName));

        /// <summary>
        /// 依据当前文件定位器的基础名与指定的扩展名，新建一个 <see cref="FileNameLocator"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="FileNameLocator"/>。</param>
        /// <param name="newExtensionFactory">给定的新扩展名工厂方法（输入参数为当前扩展名）。</param>
        /// <returns>返回 <see cref="FileNameLocator"/>。</returns>
        public static FileNameLocator NewExtension(this FileNameLocator locator, Func<string, string> newExtensionFactory)
            => locator.NotNull(nameof(locator))
            .NewExtension(newExtensionFactory?.Invoke(locator.Extension));

    }
}
