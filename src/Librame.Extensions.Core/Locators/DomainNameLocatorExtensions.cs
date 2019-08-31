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
    /// 域名定位器静态扩展。
    /// </summary>
    public static class DomainNameLocatorExtensions
    {
        /// <summary>
        /// 转换为域名定位器。
        /// </summary>
        /// <param name="domainName">给定的域名。</param>
        /// <returns>返回 <see cref="DomainNameLocator"/>。</returns>
        public static DomainNameLocator AsDomainNameLocator(this string domainName)
        {
            return (DomainNameLocator)domainName;
        }

    }
}
