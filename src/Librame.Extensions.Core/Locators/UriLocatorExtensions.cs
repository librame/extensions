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
using System.Collections.Concurrent;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// URI 定位器静态扩展。
    /// </summary>
    public static class UriLocatorExtensions
    {
        /// <summary>
        /// 转换为 URI 定位器。
        /// </summary>
        /// <param name="uriString">给定的 URI 字符串。</param>
        /// <param name="newScheme">给定的新协议（可选）。</param>
        /// <param name="newHost">给定可能包含端口号的新主机（可选）。</param>
        /// <param name="newPath">给定以 / 开始的新路径（可选）。</param>
        /// <param name="newQuery">给定以 ? 开始的新查询（可选）。</param>
        /// <param name="newAnchor">给定以 # 开始的新锚点（可选）。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        public static IUriLocator AsUriLocator(this string uriString, string newScheme = null,
            string newHost = null, string newPath = null, string newQuery = null,
            string newAnchor = null)
        {
            var locator = (UriLocator)uriString;

            return locator.ChangeParameters(newScheme, newHost, newPath,
                newQuery, queriesAction: null, newAnchor);
        }

        /// <summary>
        /// 转换为 URI 定位器。
        /// </summary>
        /// <param name="uriString">给定的 URI 字符串。</param>
        /// <param name="queriesAction">给定的改变查询参数集合动作。</param>
        /// <param name="newScheme">给定的新协议（可选）。</param>
        /// <param name="newHost">给定可能包含端口号的新主机（可选）。</param>
        /// <param name="newPath">给定以 / 开始的新路径（可选）。</param>
        /// <param name="newAnchor">给定以 # 开始的新锚点（可选）。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        public static IUriLocator AsUriLocator(this string uriString,
            Action<ConcurrentDictionary<string, string>> queriesAction, string newScheme = null,
            string newHost = null, string newPath = null, string newAnchor = null)
        {
            var locator = (UriLocator)uriString;

            return locator.ChangeParameters(newScheme, newHost, newPath,
                newQuery: default, queriesAction, newAnchor);
        }


        /// <summary>
        /// 转换为 URI 定位器。
        /// </summary>
        /// <param name="uri">给定的 URI 字符串。</param>
        /// <param name="newScheme">给定的新协议（可选）。</param>
        /// <param name="newHost">给定可能包含端口号的新主机（可选）。</param>
        /// <param name="newPath">给定以 / 开始的新路径（可选）。</param>
        /// <param name="newQuery">给定以 ? 开始的新查询（可选）。</param>
        /// <param name="newAnchor">给定以 # 开始的新锚点（可选）。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        public static IUriLocator AsUriLocator(this Uri uri, string newScheme = null,
            string newHost = null, string newPath = null, string newQuery = null,
            string newAnchor = null)
        {
            var locator = (UriLocator)uri;

            return locator.ChangeParameters(newScheme, newHost, newPath,
                newQuery, queriesAction: null, newAnchor);
        }

        /// <summary>
        /// 转换为 URI 定位器。
        /// </summary>
        /// <param name="uri">给定的 URI 字符串。</param>
        /// <param name="queriesAction">给定的改变查询参数集合动作。</param>
        /// <param name="newScheme">给定的新协议（可选）。</param>
        /// <param name="newHost">给定可能包含端口号的新主机（可选）。</param>
        /// <param name="newPath">给定以 / 开始的新路径（可选）。</param>
        /// <param name="newAnchor">给定以 # 开始的新锚点（可选）。</param>
        /// <returns>返回 <see cref="IUriLocator"/>。</returns>
        public static IUriLocator AsUriLocator(this Uri uri,
            Action<ConcurrentDictionary<string, string>> queriesAction, string newScheme = null,
            string newHost = null, string newPath = null, string newAnchor = null)
        {
            var locator = (UriLocator)uri;

            return locator.ChangeParameters(newScheme, newHost, newPath,
                newQuery: null, queriesAction, newAnchor);
        }


        private static IUriLocator ChangeParameters(this IUriLocator locator, string newScheme = null,
            string newHost = null, string newPath = null, string newQuery = null,
            Action<ConcurrentDictionary<string, string>> queriesAction = null, string newAnchor = null)
        {
            if (!newScheme.IsNullOrEmpty())
                locator.ChangeScheme(newScheme);

            if (newHost.IsNullOrEmpty())
                locator.ChangeHost(newHost);

            if (newPath.IsNullOrEmpty())
                locator.ChangePath(newPath);

            if (newQuery.IsNullOrEmpty())
                locator.ChangeQuery(newQuery);

            if (queriesAction.IsNotNull())
                locator.ChangeQueries(queriesAction);

            if (!newAnchor.IsNullOrEmpty())
                locator.ChangeAnchor(newAnchor);

            return locator;
        }

    }
}
