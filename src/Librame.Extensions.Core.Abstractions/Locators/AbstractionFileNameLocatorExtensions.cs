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
        /// 当作数组。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileNameLocator"/>。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/> 集合。</returns>
        public static IFileNameLocator[] AsArray(this IFileNameLocator locator)
        {
            return new IFileNameLocator[] { locator };
        }

        /// <summary>
        /// 转换为字符串集合。
        /// </summary>
        /// <param name="locators">给定的 <see cref="IFileNameLocator"/> 集合。</param>
        /// <returns>返回字符串集合。</returns>
        public static IEnumerable<string> ToStrings(this IEnumerable<IFileNameLocator> locators)
        {
            return locators.Select(locator => locator?.ToString());
        }


        /// <summary>
        /// 改变基础名。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileNameLocator"/>。</param>
        /// <param name="newBaseNameFactory">给定的新基础名工厂方法（输入参数为当前基础名）。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        public static IFileNameLocator ChangeBaseName(this IFileNameLocator locator, Func<string, string> newBaseNameFactory)
        {
            locator.NotNull(nameof(locator));

            return locator.ChangeBaseName(newBaseNameFactory?.Invoke(locator.BaseName));
        }

        /// <summary>
        /// 改变扩展名。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileNameLocator"/>。</param>
        /// <param name="newExtensionFactory">给定的新扩展名工厂方法（输入参数为当前扩展名）。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        public static IFileNameLocator ChangeExtension(this IFileNameLocator locator, Func<string, string> newExtensionFactory)
        {
            locator.NotNull(nameof(locator));

            return locator.ChangeExtension(newExtensionFactory?.Invoke(locator.Extension));
        }

        /// <summary>
        /// 批量改变扩展名。
        /// </summary>
        /// <param name="locators">给定的 <see cref="IFileNameLocator"/> 集合。</param>
        /// <param name="newExtension">给定的新扩展名。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/> 集合。</returns>
        public static IEnumerable<IFileNameLocator> ChangeExtension(this IEnumerable<IFileNameLocator> locators, string newExtension)
        {
            locators.NotNull(nameof(locators));

            foreach (var locator in locators)
                locator?.ChangeExtension(newExtension);

            return locators;
        }


        /// <summary>
        /// 依据当前文件定位器的扩展名与指定的基础名，新建一个 <see cref="IFileNameLocator"/> 实例。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileNameLocator"/>。</param>
        /// <param name="newBaseNameFactory">给定的新基础名工厂方法（输入参数为当前基础名）。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        public static IFileNameLocator NewBaseName(this IFileNameLocator locator, Func<string, string> newBaseNameFactory)
        {
            locator.NotNull(nameof(locator));

            return locator.NewBaseName(newBaseNameFactory?.Invoke(locator.BaseName));
        }

        /// <summary>
        /// 依据当前文件定位器的基础名与指定的扩展名，新建一个 <see cref="IFileNameLocator"/> 实例。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IFileNameLocator"/>。</param>
        /// <param name="newExtensionFactory">给定的新扩展名工厂方法（输入参数为当前扩展名）。</param>
        /// <returns>返回 <see cref="IFileNameLocator"/>。</returns>
        public static IFileNameLocator NewExtension(this IFileNameLocator locator, Func<string, string> newExtensionFactory)
        {
            locator.NotNull(nameof(locator));

            return locator.NewExtension(newExtensionFactory?.Invoke(locator.Extension));
        }

    }
}
