#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象域名定位器静态扩展。
    /// </summary>
    public static class AbstractionDomainNameLocatorExtensions
    {
        /// <summary>
        /// 获取仅两级域名形式。
        /// </summary>
        /// <param name="locator">给定的 <see cref="IDomainNameLocator"/>。</param>
        /// <returns>返回包含子级与父级的两级元组。</returns>
        public static (string Child, string Parent) GetOnlyTwoLevels(this IDomainNameLocator locator)
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
