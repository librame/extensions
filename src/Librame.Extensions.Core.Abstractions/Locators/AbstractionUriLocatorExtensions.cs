#region License

/* **************************************************************************************
 * Copyright (c) zwbwl All rights reserved.
 * 
 * http://51zwb.com
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Collections.Concurrent;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象 URI 定位器静态扩展。
    /// </summary>
    public static class AbstractionUriLocatorExtensions
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
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator AsUriLocator(this string uriString, string newScheme = null,
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
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator AsUriLocator(this string uriString,
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
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator AsUriLocator(this Uri uri, string newScheme = null,
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
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator AsUriLocator(this Uri uri,
            Action<ConcurrentDictionary<string, string>> queriesAction, string newScheme = null,
            string newHost = null, string newPath = null, string newAnchor = null)
        {
            var locator = (UriLocator)uri;

            return locator.ChangeParameters(newScheme, newHost, newPath,
                newQuery: null, queriesAction, newAnchor);
        }


        private static UriLocator ChangeParameters(this UriLocator locator, string newScheme = null,
            string newHost = null, string newPath = null, string newQuery = null,
            Action<ConcurrentDictionary<string, string>> queriesAction = null, string newAnchor = null)
        {
            if (newScheme.IsNotNullOrEmpty())
                locator.ChangeScheme(newScheme);

            if (newHost.HasValue)
                locator.ChangeHost(newHost);

            if (newPath.HasValue)
                locator.ChangePath(newPath);

            if (newQuery.HasValue)
                locator.ChangeQuery(newQuery);

            if (queriesAction.IsNotNull())
                locator.ChangeQueries(queriesAction);

            if (newAnchor.IsNotNullOrEmpty())
                locator.ChangeAnchor(newAnchor);

            return locator;
        }


        /// <summary>
        /// 改变协议。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        /// <param name="newSchemeFactory">给定的新协议字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator ChangeScheme(this UriLocator locator, Func<string, string> newSchemeFactory)
        {
            locator.NotNull(nameof(locator));
            newSchemeFactory.NotNull(nameof(newSchemeFactory));

            var newScheme = newSchemeFactory.Invoke(locator.Scheme);
            return locator.ChangeScheme(newScheme);
        }

        /// <summary>
        /// 改变主机。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        /// <param name="newHostFactory">给定可能包含端口号的新主机字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator ChangeHost(this UriLocator locator, Func<string, string> newHostFactory)
        {
            locator.NotNull(nameof(locator));
            newHostFactory.NotNull(nameof(newHostFactory));

            var newHost = newHostFactory.Invoke(locator.Host);
            return locator.ChangeHost(newHost);
        }

        /// <summary>
        /// 改变路径。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        /// <param name="newPathFactory">给定以 / 开始的新路径字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator ChangePath(this UriLocator locator, Func<string, string> newPathFactory)
        {
            locator.NotNull(nameof(locator));
            newPathFactory.NotNull(nameof(newPathFactory));

            var newPath = newPathFactory.Invoke(locator.Path);
            return locator.ChangePath(newPath);
        }

        /// <summary>
        /// 改变查询字符串。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        /// <param name="newQueryFactory">给定以 ? 开始的新查询字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator ChangeQuery(this UriLocator locator, Func<string, string> newQueryFactory)
        {
            locator.NotNull(nameof(locator));
            newQueryFactory.NotNull(nameof(newQueryFactory));

            var newQuery = newQueryFactory.Invoke(locator.Query);
            return locator.ChangeQuery(newQuery);
        }

        /// <summary>
        /// 改变锚点。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        /// <param name="newAnchorFactory">给定以 # 开始的新锚点工厂方法。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator ChangeAnchor(this UriLocator locator, Func<string, string> newAnchorFactory)
        {
            locator.NotNull(nameof(locator));
            newAnchorFactory.NotNull(nameof(newAnchorFactory));

            var newAnchor = newAnchorFactory.Invoke(locator.Anchor);
            return locator.ChangeAnchor(newAnchor);
        }


        /// <summary>
        /// 使用指定的新协议新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        /// <param name="newSchemeFactory">给定的新协议字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator NewScheme(this UriLocator locator, Func<string, string> newSchemeFactory)
        {
            locator.NotNull(nameof(locator));
            newSchemeFactory.NotNull(nameof(newSchemeFactory));

            var newScheme = newSchemeFactory.Invoke(locator.Scheme);
            return locator.NewScheme(newScheme);
        }

        /// <summary>
        /// 使用指定的新主机新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        /// <param name="newHostFactory">给定可能包含端口号的新主机字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator NewHost(this UriLocator locator, Func<string, string> newHostFactory)
        {
            locator.NotNull(nameof(locator));
            newHostFactory.NotNull(nameof(newHostFactory));

            var newHost = newHostFactory.Invoke(locator.Host);
            return locator.NewHost(newHost);
        }

        /// <summary>
        /// 使用指定的新路径新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        /// <param name="newPathFactory">给定以 / 开始的新路径字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator NewPath(this UriLocator locator, Func<string, string> newPathFactory)
        {
            locator.NotNull(nameof(locator));
            newPathFactory.NotNull(nameof(newPathFactory));

            var newPath = newPathFactory.Invoke(locator.Path);
            return locator.NewPath(newPath);
        }

        /// <summary>
        /// 使用指定的新查询字符串新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        /// <param name="newQueryFactory">给定以 ? 开始的新查询字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator NewQuery(this UriLocator locator, Func<string, string> newQueryFactory)
        {
            locator.NotNull(nameof(locator));
            newQueryFactory.NotNull(nameof(newQueryFactory));

            var newQuery = newQueryFactory.Invoke(locator.Query);
            return locator.NewQuery(newQuery);
        }

        /// <summary>
        /// 使用指定的新锚点新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        /// <param name="newAnchorFactory">给定以 # 开始的新锚点字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public static UriLocator NewAnchor(this UriLocator locator, Func<string, string> newAnchorFactory)
        {
            locator.NotNull(nameof(locator));
            newAnchorFactory.NotNull(nameof(newAnchorFactory));

            var newAnchor = newAnchorFactory.Invoke(locator.Anchor);
            return locator.NewAnchor(newAnchor);
        }

    }
}
