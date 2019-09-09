﻿#region License

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
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// URI 定位器。
    /// </summary>
    public class UriLocator : AbstractLocator<Uri>
    {
        /// <summary>
        /// 构造一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        public UriLocator(Uri uri)
            : base(uri)
        {
            Scheme = uri.Scheme;
            Host = uri.Authority;
            Path = uri.AbsolutePath; // uri.LocalPath
            Query = uri.Query;
            Anchor = uri.Fragment;
        }

        /// <summary>
        /// 构造一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="scheme"/> or <paramref name="host"/> is null or empty.
        /// </exception>
        /// <param name="scheme">给定的协议。</param>
        /// <param name="host">给定可能包含端口号的主机。</param>
        /// <param name="path">给定以 / 开始的路径（可选）。</param>
        /// <param name="query">给定以 ? 开始的查询（可选）。</param>
        /// <param name="anchor">给定以 # 开始的锚点（可选）。</param>
        protected UriLocator(string scheme, string host,
            string path = null, string query = null, string anchor = null)
            : base(CombineUri(scheme, host, path, query, anchor))
        {
            Scheme = scheme;
            Host = host;
            Path = path;
            Query = query;
            Anchor = anchor;
        }


        /// <summary>
        /// 协议。
        /// </summary>
        public string Scheme { get; private set; }

        /// <summary>
        /// 可能包含端口号的主机。
        /// </summary>
        public string Host { get; private set; }

        /// <summary>
        /// 以 / 开始的路径。
        /// </summary>
        public string Path { get; private set; }

        /// <summary>
        /// 以 ? 开始的查询。
        /// </summary>
        public string Query { get; private set; }

        /// <summary>
        /// 以 # 开始的锚点。
        /// </summary>
        public string Anchor { get; private set; }

        /// <summary>
        /// 域名定位器。
        /// </summary>
        /// <value>返回 <see cref="DomainNameLocator"/>。</value>
        public DomainNameLocator DomainName
        {
            get
            {
                if (DomainNameLocator.TryParseAllLevelSegmentsFromHost(Host,
                    out IEnumerable<string> allLevelSegments))
                {
                    return allLevelSegments.ToList().AsDomainNameLocator();
                }

                return null;
            }
        }

        /// <summary>
        /// 查询参数集合。
        /// </summary>
        public ConcurrentDictionary<string, string> Queries
            => FromQuery(Query);


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override Uri Source
        {
            get { return CombineUri(Scheme, Host, Path, Query, Anchor); }
        }


        /// <summary>
        /// 改变协议。
        /// </summary>
        /// <param name="newScheme">给定的新协议。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator ChangeScheme(string newScheme)
        {
            Scheme = newScheme.NotNullOrEmpty(nameof(newScheme));
            return this;
        }

        /// <summary>
        /// 改变主机。
        /// </summary>
        /// <param name="newHost">给定可能包含端口号的新主机。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator ChangeHost(string newHost)
        {
            Host = newHost.NotNullOrEmpty(nameof(newHost));
            return this;
        }

        /// <summary>
        /// 改变路径。
        /// </summary>
        /// <param name="newPath">给定以 / 开始的新路径。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator ChangePath(string newPath)
        {
            Path = newPath;
            return this;
        }

        /// <summary>
        /// 改变查询。
        /// </summary>
        /// <param name="newQuery">给定以 ? 开始的新查询。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator ChangeQuery(string newQuery)
        {
            Query = newQuery;
            return this;
        }

        /// <summary>
        /// 改变查询参数集合。
        /// </summary>
        /// <param name="queriesAction">给定的改变查询参数集合动作（内部支持对参数值的特殊字符进行转码处理）。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator ChangeQueries(Action<ConcurrentDictionary<string, string>> queriesAction)
        {
            queriesAction.NotNull(nameof(queriesAction)).Invoke(Queries);

            var queryString = ToQuery(Queries);
            return ChangeQuery(queryString);
        }

        /// <summary>
        /// 改变锚点。
        /// </summary>
        /// <param name="newAnchor">给定以 # 开始的新锚点。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator ChangeAnchor(string newAnchor)
        {
            Anchor = newAnchor;
            return this;
        }


        /// <summary>
        /// 使用指定的新协议新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="newScheme">给定的新协议。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator NewScheme(string newScheme)
            => new UriLocator(newScheme, Host, Path, Query, Anchor);

        /// <summary>
        /// 使用指定的新主机新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="newHost">给定可能包含端口号的新主机。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator NewHost(string newHost)
            => new UriLocator(Scheme, newHost, Path, Query, Anchor);

        /// <summary>
        /// 使用指定的新路径新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="newPath">给定以 / 开始的新路径。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator NewPath(string newPath)
            => new UriLocator(Scheme, Host, newPath, Query, Anchor);

        /// <summary>
        /// 使用指定的新查询新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="newQuery">给定以 ? 开始的新查询。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator NewQuery(string newQuery)
            => new UriLocator(Scheme, Host, Path, newQuery, Anchor);

        /// <summary>
        /// 使用指定的查询参数数组新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="queriesAction">给定的改变查询参数集合动作（内部支持对参数值的特殊字符进行转码处理）。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator NewQueries(Action<ConcurrentDictionary<string, string>> queriesAction)
        {
            var queries = FromQuery(Query.ToString());
            queriesAction.Invoke(queries);

            var queryString = ToQuery(queries);
            return new UriLocator(Scheme, Host, Path, queryString, Anchor);
        }

        /// <summary>
        /// 使用指定的新锚点新建一个 <see cref="UriLocator"/>。
        /// </summary>
        /// <param name="newAnchor">给定以 # 开始的新锚点。</param>
        /// <returns>返回 <see cref="UriLocator"/>。</returns>
        public UriLocator NewAnchor(string newAnchor)
            => new UriLocator(Scheme, Host, Path, Query, newAnchor);


        /// <summary>
        /// 比较是否相等。
        /// </summary>
        /// <param name="other">给定的 <see cref="UriLocator"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool Equals(UriLocator other)
            => Source == other.Source;


        /// <summary>
        /// 转换为文件。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Source.ToString();


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="locator">给定的 <see cref="UriLocator"/>。</param>
        public static implicit operator string(UriLocator locator)
            => locator.ToString();

        /// <summary>
        /// 显式转换为 URI 定位器。
        /// </summary>
        /// <param name="uriString">给定的绝对 URI 字符串。</param>
        public static explicit operator UriLocator(string uriString)
        {
            if (uriString.IsAbsoluteUri(out Uri result))
                return new UriLocator(result);

            throw new ArgumentException($"Invalid absolute uri string: {uriString}.");
        }
        /// <summary>
        /// 显式转换为 URI 定位器。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        public static explicit operator UriLocator(Uri uri)
            => new UriLocator(uri);


        /// <summary>
        /// 组合 URI。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="scheme"/> or <paramref name="host"/> is null or empty.
        /// </exception>
        /// <param name="scheme">给定的协议。</param>
        /// <param name="host">给定的主机。</param>
        /// <param name="path">给定的路径（可选）。</param>
        /// <param name="query">给定的查询（可选）。</param>
        /// <param name="anchor">给定以 # 开始的锚点（可选）。</param>
        /// <returns>返回 <see cref="Uri"/>。</returns>
        public static Uri CombineUri(string scheme, string host,
            string path = default, string query = default, string anchor = null)
        {
            scheme.NotNullOrEmpty(nameof(scheme));
            host.NotNullOrEmpty(nameof(host));

            return new Uri($"{scheme}{Uri.SchemeDelimiter}{host}{path}{query}{anchor}");
        }


        /// <summary>
        /// 从查询字符串还原查询参数集合（内部支持对参数值的特殊字符进行解码处理）。
        /// </summary>
        /// <param name="queryString">给定的查询参数字符串。</param>
        /// <returns>返回 <see cref="ConcurrentDictionary{String, String}"/>。</returns>
        public static ConcurrentDictionary<string, string> FromQuery(string queryString)
        {
            var queries = new ConcurrentDictionary<string, string>();

            if (queryString.IsNotNullOrEmpty())
            {
                queryString.TrimStart('?').Split('&').ForEach(segment =>
                {
                    if (segment.IsNotNullOrEmpty())
                    {
                        var pair = segment.SplitPair(); // "="
                        var valueString = pair.Value;

                        if (valueString.IsNotNullOrEmpty())
                            valueString = Uri.UnescapeDataString(valueString);

                        queries.AddOrUpdate(pair.Key, valueString, (key, value) => valueString);
                    }
                });
            }

            return queries;
        }

        /// <summary>
        /// 将查询参数集合转换为查询字符串（内部支持对参数值的特殊字符进行转码处理）。
        /// </summary>
        /// <param name="queries">给定的查询参数集合。</param>
        /// <returns>返回查询字符串。</returns>
        public static string ToQuery(IEnumerable<KeyValuePair<string, string>> queries)
        {
            var sb = new StringBuilder("?");

            var count = queries.Count();
            queries.ForEach((pair, i) =>
            {
                sb.Append(pair.Key);
                sb.Append("=");

                if (pair.Value.IsNotNullOrEmpty())
                    sb.Append(Uri.EscapeDataString(pair.Value));

                if (i < count - 1)
                    sb.Append("&");
            });

            return Uri.EscapeUriString(sb.ToString());
        }

    }
}