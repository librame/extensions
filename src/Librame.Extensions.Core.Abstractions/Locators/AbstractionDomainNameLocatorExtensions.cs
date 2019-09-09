#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象域名定位器静态扩展。
    /// </summary>
    public static class AbstractionDomainNameLocatorExtensions
    {
        /// <summary>
        /// 转换为域名定位器。
        /// </summary>
        /// <param name="host">给定的主机。</param>
        /// <returns>返回 <see cref="DomainNameLocator"/>。</returns>
        public static DomainNameLocator AsDomainNameLocator(this string host)
        {
            return (DomainNameLocator)host;
        }

        /// <summary>
        /// 转换为域名定位器。
        /// </summary>
        /// <param name="allLevelSegments">给定的所有级别片段列表。</param>
        /// <returns>返回 <see cref="DomainNameLocator"/>。</returns>
        public static DomainNameLocator AsDomainNameLocator(this List<string> allLevelSegments)
        {
            return new DomainNameLocator(allLevelSegments);
        }

        /// <summary>
        /// 获取仅两级域名形式。
        /// </summary>
        /// <param name="locator">给定的 <see cref="DomainNameLocator"/>。</param>
        /// <returns>返回包含子级与父级的两级元组。</returns>
        public static (string Child, string Parent) GetOnlyTwoLevels(this DomainNameLocator locator)
        {
            locator.NotNull(nameof(locator));

            if (locator.TopLevelSegment.IsNullOrEmpty())
                return (null, locator.Root);

            if (locator.SecondLevelSegment.IsNullOrEmpty())
                return (null, locator.TopLevel);

            var child = locator.Source.TrimEnd($".{locator.TopLevel}");
            return (child, locator.TopLevel);
        }

    }
}
