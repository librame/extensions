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
    /// 文件定位器静态扩展。
    /// </summary>
    public static class FileLocatorExtensions
    {
        /// <summary>
        /// 转换为文件定位器。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator AsFileLocator(this string fileName, string basePath = null)
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
        /// <returns>返回 <see cref="IFileLocator"/> 数组。</returns>
        public static IFileLocator[] AsFileLocators(this string[] fileNames, string basePath = null)
        {
            var locators = new IFileLocator[fileNames.Length];

            for (var i = 0; i < fileNames.Length; i++)
                locators[i] = fileNames[i].AsFileLocator(basePath);

            return locators;
        }

    }
}
