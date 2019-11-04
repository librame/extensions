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
using System.Diagnostics.CodeAnalysis;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 抽象 URL 组合器静态扩展。
    /// </summary>
    public static class AbstractionUriCombinerExtensions
    {
        /// <summary>
        /// 转换为 URL 组合器。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <param name="newScheme">给定的新协议（可选）。</param>
        /// <param name="newHost">给定可能包含端口号的新主机（可选）。</param>
        /// <param name="newPath">给定以 / 开始的新路径（可选）。</param>
        /// <param name="newQuery">给定以 ? 开始的新查询（可选）。</param>
        /// <param name="newAnchor">给定以 # 开始的新锚点（可选）。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public static UriCombiner AsUriCombiner(this string absoluteUriString, string newScheme = null,
            string newHost = null, string newPath = null, string newQuery = null,
            string newAnchor = null)
        {
            var combiner = new UriCombiner(absoluteUriString);

            return combiner.ChangeParameters(newScheme, newHost, newPath,
                newQuery, queriesAction: null, newAnchor);
        }

        /// <summary>
        /// 转换为 URL 组合器。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        /// <param name="queriesAction">给定的改变查询参数集合动作。</param>
        /// <param name="newScheme">给定的新协议（可选）。</param>
        /// <param name="newHost">给定可能包含端口号的新主机（可选）。</param>
        /// <param name="newPath">给定以 / 开始的新路径（可选）。</param>
        /// <param name="newAnchor">给定以 # 开始的新锚点（可选）。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public static UriCombiner AsUriCombiner(this string absoluteUriString,
            Action<ConcurrentDictionary<string, string>> queriesAction, string newScheme = null,
            string newHost = null, string newPath = null, string newAnchor = null)
        {
            var combiner = new UriCombiner(absoluteUriString);

            return combiner.ChangeParameters(newScheme, newHost, newPath,
                newQuery: default, queriesAction, newAnchor);
        }


        /// <summary>
        /// 转换为 URL 组合器。
        /// </summary>
        /// <param name="uri">给定的 URI 字符串。</param>
        /// <param name="newScheme">给定的新协议（可选）。</param>
        /// <param name="newHost">给定可能包含端口号的新主机（可选）。</param>
        /// <param name="newPath">给定以 / 开始的新路径（可选）。</param>
        /// <param name="newQuery">给定以 ? 开始的新查询（可选）。</param>
        /// <param name="newAnchor">给定以 # 开始的新锚点（可选）。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public static UriCombiner AsUriCombiner(this Uri uri, string newScheme = null,
            string newHost = null, string newPath = null, string newQuery = null,
            string newAnchor = null)
        {
            var combiner = new UriCombiner(uri);

            return combiner.ChangeParameters(newScheme, newHost, newPath,
                newQuery, queriesAction: null, newAnchor);
        }

        /// <summary>
        /// 转换为 URL 组合器。
        /// </summary>
        /// <param name="uri">给定的 URI 字符串。</param>
        /// <param name="queriesAction">给定的改变查询参数集合动作。</param>
        /// <param name="newScheme">给定的新协议（可选）。</param>
        /// <param name="newHost">给定可能包含端口号的新主机（可选）。</param>
        /// <param name="newPath">给定以 / 开始的新路径（可选）。</param>
        /// <param name="newAnchor">给定以 # 开始的新锚点（可选）。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public static UriCombiner AsUriCombiner(this Uri uri,
            Action<ConcurrentDictionary<string, string>> queriesAction, string newScheme = null,
            string newHost = null, string newPath = null, string newAnchor = null)
        {
            var combiner = new UriCombiner(uri);

            return combiner.ChangeParameters(newScheme, newHost, newPath,
                newQuery: null, queriesAction, newAnchor);
        }


        private static UriCombiner ChangeParameters(this UriCombiner combiner, string newScheme = null,
            string newHost = null, string newPath = null, string newQuery = null,
            Action<ConcurrentDictionary<string, string>> queriesAction = null, string newAnchor = null)
        {
            if (newScheme.IsNotEmpty())
                combiner.ChangeScheme(newScheme);

            if (newHost.IsNotEmpty())
                combiner.ChangeHost(newHost);

            if (newPath.IsNotEmpty())
                combiner.ChangePath(newPath);

            if (newQuery.IsNotEmpty())
                combiner.ChangeQuery(newQuery);

            if (queriesAction.IsNotNull())
                combiner.ChangeQueries(queriesAction);

            if (newAnchor.IsNotEmpty())
                combiner.ChangeAnchor(newAnchor);

            return combiner;
        }


        /// <summary>
        /// 改变协议。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="newSchemeFactory">给定的新协议字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static UriCombiner ChangeScheme(this UriCombiner combiner, Func<string, string> newSchemeFactory)
        {
            combiner.NotNull(nameof(combiner));
            newSchemeFactory.NotNull(nameof(newSchemeFactory));

            var newScheme = newSchemeFactory.Invoke(combiner.Scheme);
            return combiner.ChangeScheme(newScheme);
        }

        /// <summary>
        /// 改变主机。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="newHostFactory">给定可能包含端口号的新主机字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static UriCombiner ChangeHost(this UriCombiner combiner, Func<string, string> newHostFactory)
        {
            combiner.NotNull(nameof(combiner));
            newHostFactory.NotNull(nameof(newHostFactory));

            var newHost = newHostFactory.Invoke(combiner.Host);
            return combiner.ChangeHost(newHost);
        }

        /// <summary>
        /// 改变路径。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="newPathFactory">给定以 / 开始的新路径字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static UriCombiner ChangePath(this UriCombiner combiner, Func<string, string> newPathFactory)
        {
            combiner.NotNull(nameof(combiner));
            newPathFactory.NotNull(nameof(newPathFactory));

            var newPath = newPathFactory.Invoke(combiner.Path);
            return combiner.ChangePath(newPath);
        }

        /// <summary>
        /// 改变查询字符串。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="newQueryFactory">给定以 ? 开始的新查询字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static UriCombiner ChangeQuery(this UriCombiner combiner, Func<string, string> newQueryFactory)
        {
            combiner.NotNull(nameof(combiner));
            newQueryFactory.NotNull(nameof(newQueryFactory));

            var newQuery = newQueryFactory.Invoke(combiner.Query);
            return combiner.ChangeQuery(newQuery);
        }

        /// <summary>
        /// 改变锚点。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="newAnchorFactory">给定以 # 开始的新锚点工厂方法。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static UriCombiner ChangeAnchor(this UriCombiner combiner, Func<string, string> newAnchorFactory)
        {
            combiner.NotNull(nameof(combiner));
            newAnchorFactory.NotNull(nameof(newAnchorFactory));

            var newAnchor = newAnchorFactory.Invoke(combiner.Anchor);
            return combiner.ChangeAnchor(newAnchor);
        }


        /// <summary>
        /// 使用指定的新协议新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="newSchemeFactory">给定的新协议字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static UriCombiner NewScheme(this UriCombiner combiner, Func<string, string> newSchemeFactory)
        {
            combiner.NotNull(nameof(combiner));
            newSchemeFactory.NotNull(nameof(newSchemeFactory));

            var newScheme = newSchemeFactory.Invoke(combiner.Scheme);
            return combiner.NewScheme(newScheme);
        }

        /// <summary>
        /// 使用指定的新主机新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="newHostFactory">给定可能包含端口号的新主机字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static UriCombiner NewHost(this UriCombiner combiner, Func<string, string> newHostFactory)
        {
            combiner.NotNull(nameof(combiner));
            newHostFactory.NotNull(nameof(newHostFactory));

            var newHost = newHostFactory.Invoke(combiner.Host);
            return combiner.NewHost(newHost);
        }

        /// <summary>
        /// 使用指定的新路径新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="newPathFactory">给定以 / 开始的新路径字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static UriCombiner NewPath(this UriCombiner combiner, Func<string, string> newPathFactory)
        {
            combiner.NotNull(nameof(combiner));
            newPathFactory.NotNull(nameof(newPathFactory));

            var newPath = newPathFactory.Invoke(combiner.Path);
            return combiner.NewPath(newPath);
        }

        /// <summary>
        /// 使用指定的新查询字符串新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="newQueryFactory">给定以 ? 开始的新查询字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static UriCombiner NewQuery(this UriCombiner combiner, Func<string, string> newQueryFactory)
        {
            combiner.NotNull(nameof(combiner));
            newQueryFactory.NotNull(nameof(newQueryFactory));

            var newQuery = newQueryFactory.Invoke(combiner.Query);
            return combiner.NewQuery(newQuery);
        }

        /// <summary>
        /// 使用指定的新锚点新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="newAnchorFactory">给定以 # 开始的新锚点字符串工厂方法。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static UriCombiner NewAnchor(this UriCombiner combiner, Func<string, string> newAnchorFactory)
        {
            combiner.NotNull(nameof(combiner));
            newAnchorFactory.NotNull(nameof(newAnchorFactory));

            var newAnchor = newAnchorFactory.Invoke(combiner.Anchor);
            return combiner.NewAnchor(newAnchor);
        }

    }
}
