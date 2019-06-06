#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Linq;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象文件定位器静态扩展。
    /// </summary>
    public static class AbstractFileLocatorExtensions
    {
        /// <summary>
        /// 当作数组。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileLocator"/>。</param>
        /// <returns>返回 <see cref="IFileLocator"/> 数组。</returns>
        public static IFileLocator[] AsArray(this IFileLocator locator)
        {
            return new IFileLocator[] { locator };
        }

        /// <summary>
        /// 转换为字符串数组。
        /// </summary>
        /// <param name="locators">给定的 <see cref="IFileLocator"/> 数组。</param>
        /// <returns>返回字符串数组。</returns>
        public static string[] ToStrings(this IFileLocator[] locators)
        {
            return locators.Select(locator => locator?.GetSource()).ToArray();
        }

    }
}
