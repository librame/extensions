#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 文件名定位器静态扩展。
    /// </summary>
    public static class FileNameLocatorExtensions
    {
        /// <summary>
        /// 转换为文件名定位器。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static FileNameLocator AsFileNameLocator(this string fileName)
        {
            return (FileNameLocator)fileName;
        }
        /// <summary>
        /// 转换为文件名定位器。
        /// </summary>
        /// <param name="baseName">给定的基础名。</param>
        /// <param name="extension">给定的扩展名。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static FileNameLocator AsFileNameLocator(this string baseName, string extension = null)
        {
            return new FileNameLocator(baseName, extension);
        }

        /// <summary>
        /// 将文件名数组转换为文件名定位器数组。
        /// </summary>
        /// <param name="fileNames">给定的文件名数组。</param>
        /// <returns>返回 <see cref="IFileLocator"/> 数组。</returns>
        public static FileNameLocator[] AsFileNameLocators(this string[] fileNames)
        {
            var locators = new FileNameLocator[fileNames.Length];

            for (var i = 0; i < fileNames.Length; i++)
                locators[i] = fileNames[i].AsFileNameLocator();

            return locators;
        }

    }
}
