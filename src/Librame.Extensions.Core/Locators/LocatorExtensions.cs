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
    /// 定位器静态扩展。
    /// </summary>
    public static class LocatorExtensions
    {
        /// <summary>
        /// 转换为默认文件定位器。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <param name="basePath">给定的基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator AsFileLocator(this string fileName, string basePath = null)
        {
            var locator = new FileLocator(fileName);

            if (!basePath.IsNullOrEmpty())
                locator.ChangeBasePath(basePath);

            return locator;
        }


        /// <summary>
        /// 依据当前文件定位器的文件名与指定的基础路径，新建一个文件定位器。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="basePath">指定的基础路径。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator NewBasePath(this IFileLocator locator, string basePath)
        {
            return locator.FileName.AsFileLocator(basePath);
        }

        /// <summary>
        /// 依据当前文件定位器的基础路径与指定的文件名，新建一个文件定位器。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <param name="fileName">指定的文件名。</param>
        /// <returns>返回 <see cref="IFileLocator"/>。</returns>
        public static IFileLocator NewFileName(this IFileLocator locator, string fileName)
        {
            return fileName.AsFileLocator(locator.BasePath);
        }

    }
}
