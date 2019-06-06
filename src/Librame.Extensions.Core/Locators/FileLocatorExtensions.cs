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

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 文件定位器静态扩展。
    /// </summary>
    public static class FileLocatorExtensions
    {
        /// <summary>
        /// 转换为默认文件定位器。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator AsFileLocator(this string fileName, string basePath = null)
        {
            if (!basePath.IsNullOrEmpty())
                return new FileLocator(fileName, basePath);
            else
                return new FileLocator(fileName);
        }


        /// <summary>
        /// 依据当前文件定位器的文件名与指定的基础路径，新建一个文件定位器。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="newBasePathFactory">给定的新基础路径工厂方法（输入参数为当前基础路径）。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator NewBasePath(this IFileLocator locator, Func<string, string> newBasePathFactory)
        {
            return locator.NewBasePath(newBasePathFactory?.Invoke(locator?.BasePath));
        }

        /// <summary>
        /// 依据当前文件定位器的文件名与指定的基础路径，新建一个文件定位器。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="newBasePath">给定的新基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator NewBasePath(this IFileLocator locator, string newBasePath)
        {
            locator.NotNull(nameof(locator));

            return new FileLocator(locator.FileName, newBasePath);
        }


        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个文件定位器。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="newFileNameFactory">给定的新文件名工厂方法（输入参数为当前文件名）。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator NewFileName(this IFileLocator locator, Func<string, string> newFileNameFactory)
        {
            return locator.NewFileName(newFileNameFactory?.Invoke(locator?.FileName));
        }

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个文件定位器。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="newFileName">给定的新文件名。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator NewFileName(this IFileLocator locator, string newFileName)
        {
            locator.NotNull(nameof(locator));

            return new FileLocator(newFileName, locator.BasePath);
        }

    }
}
