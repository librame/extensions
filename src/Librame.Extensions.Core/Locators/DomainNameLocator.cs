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
    /// URI 定位器。
    /// </summary>
    public class DomainNameLocator : AbstractDomainNameLocator
    {
        /// <summary>
        /// 构造一个 <see cref="DomainNameLocator"/>。
        /// </summary>
        /// <param name="domainName">给定的域名。</param>
        public DomainNameLocator(string domainName)
            : base(domainName)
        {
        }

        /// <summary>
        /// 构造一个 <see cref="DomainNameLocator"/>。
        /// </summary>
        /// <param name="allLevelSegments">给定的所有级别片段列表。</param>
        protected internal DomainNameLocator(List<string> allLevelSegments)
            : base(allLevelSegments)
        {
        }


        /// <summary>
        /// 创建定位器。
        /// </summary>
        /// <param name="copyAllLevelSegments">给定的所有级别片段列表副本。</param>
        /// <returns>返回 <see cref="IDomainNameLocator"/>。</returns>
        protected override AbstractDomainNameLocator CreateLocator(List<string> copyAllLevelSegments)
        {
            return new DomainNameLocator(copyAllLevelSegments);
        }


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="locator">给定的 <see cref="DomainNameLocator"/>。</param>
        public static implicit operator string(DomainNameLocator locator)
        {
            return locator.ToString();
        }

        /// <summary>
        /// 显式转换为域名定位器。
        /// </summary>
        /// <param name="domainName">给定的域名。</param>
        public static explicit operator DomainNameLocator(string domainName)
        {
            return new DomainNameLocator(domainName);
        }

    }
}
