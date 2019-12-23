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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;

namespace Librame.Extensions.Core.Combiners
{
    /// <summary>
    /// URI 组合器。
    /// </summary>
    public class UriCombiner : AbstractCombiner<Uri>
    {
        /// <summary>
        /// 构造一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="absoluteUriString">给定的绝对 URI 字符串。</param>
        public UriCombiner(string absoluteUriString)
            : this(absoluteUriString.AsAbsoluteUri())
        {
        }

        /// <summary>
        /// 构造一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="uri">给定的 <see cref="Uri"/>。</param>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public UriCombiner(Uri uri)
            : base(uri)
        {
            Scheme = uri.Scheme;
            Host = uri.Authority;
            Path = uri.AbsolutePath; // uri.LocalPath
            Query = uri.Query;
            Anchor = uri.Fragment;
        }

        /// <summary>
        /// 构造一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="scheme"/> or <paramref name="host"/> is null or empty.
        /// </exception>
        /// <param name="scheme">给定的协议。</param>
        /// <param name="host">给定可能包含端口号的主机。</param>
        /// <param name="path">给定以 / 开始的路径（可选）。</param>
        /// <param name="query">给定以 ? 开始的查询（可选）。</param>
        /// <param name="anchor">给定以 # 开始的锚点（可选）。</param>
        public UriCombiner(string scheme, string host,
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
        /// 域名组合器。
        /// </summary>
        /// <value>返回 <see cref="DomainNameCombiner"/>。</value>
        public DomainNameCombiner DomainName
            => CreateDomainNameCombiner(Host);

        /// <summary>
        /// 查询参数集合（不为 NULL）。
        /// </summary>
        public ConcurrentDictionary<string, string> Queries
            => FromQuery(Query);


        /// <summary>
        /// 重写源实例。
        /// </summary>
        public override Uri Source
            => CombineUri(Scheme, Host, Path, Query, Anchor);


        /// <summary>
        /// 改变协议。
        /// </summary>
        /// <param name="newScheme">给定的新协议。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner ChangeScheme(string newScheme)
        {
            Scheme = newScheme.NotEmpty(nameof(newScheme));
            return this;
        }

        /// <summary>
        /// 改变主机。
        /// </summary>
        /// <param name="newHost">给定可能包含端口号的新主机。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner ChangeHost(string newHost)
        {
            Host = newHost.NotEmpty(nameof(newHost));
            return this;
        }

        /// <summary>
        /// 改变路径。
        /// </summary>
        /// <param name="newPath">给定以 / 开始的新路径。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner ChangePath(string newPath)
        {
            Path = newPath;
            return this;
        }

        /// <summary>
        /// 改变查询。
        /// </summary>
        /// <param name="newQuery">给定以 ? 开始的新查询。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner ChangeQuery(string newQuery)
        {
            Query = newQuery;
            return this;
        }

        /// <summary>
        /// 改变查询参数集合。
        /// </summary>
        /// <param name="queriesAction">给定的改变查询参数集合动作（内部支持对参数值的特殊字符进行转码处理）。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner ChangeQueries(Action<ConcurrentDictionary<string, string>> queriesAction)
        {
            queriesAction.NotNull(nameof(queriesAction)).Invoke(Queries);

            var queryString = ToQuery(Queries);
            return ChangeQuery(queryString);
        }

        /// <summary>
        /// 改变锚点。
        /// </summary>
        /// <param name="newAnchor">给定以 # 开始的新锚点。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner ChangeAnchor(string newAnchor)
        {
            Anchor = newAnchor;
            return this;
        }


        /// <summary>
        /// 使用指定的新协议新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="newScheme">给定的新协议。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner NewScheme(string newScheme)
            => new UriCombiner(newScheme, Host, Path, Query, Anchor);

        /// <summary>
        /// 使用指定的新主机新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="newHost">给定可能包含端口号的新主机。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner NewHost(string newHost)
            => new UriCombiner(Scheme, newHost, Path, Query, Anchor);

        /// <summary>
        /// 使用指定的新路径新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="newPath">给定以 / 开始的新路径。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner NewPath(string newPath)
            => new UriCombiner(Scheme, Host, newPath, Query, Anchor);

        /// <summary>
        /// 使用指定的新查询新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="newQuery">给定以 ? 开始的新查询。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner NewQuery(string newQuery)
            => new UriCombiner(Scheme, Host, Path, newQuery, Anchor);

        /// <summary>
        /// 使用指定的查询参数数组新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="queriesAction">给定的改变查询参数集合动作（内部支持对参数值的特殊字符进行转码处理）。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods", MessageId = "queriesAction")]
        public UriCombiner NewQueries(Action<ConcurrentDictionary<string, string>> queriesAction)
        {
            queriesAction.NotNull(nameof(queriesAction));

            var queries = FromQuery(Query.ToString(CultureInfo.InvariantCulture));
            queriesAction.Invoke(queries);

            var queryString = ToQuery(queries);
            return new UriCombiner(Scheme, Host, Path, queryString, Anchor);
        }

        /// <summary>
        /// 使用指定的新锚点新建一个 <see cref="UriCombiner"/>。
        /// </summary>
        /// <param name="newAnchor">给定以 # 开始的新锚点。</param>
        /// <returns>返回 <see cref="UriCombiner"/>。</returns>
        public UriCombiner NewAnchor(string newAnchor)
            => new UriCombiner(Scheme, Host, Path, Query, newAnchor);
        

        /// <summary>
        /// 创建域名组合器。
        /// </summary>
        /// <param name="host">给定的主机。</param>
        /// <returns>返回 <see cref="DomainNameCombiner"/>。</returns>
        private static DomainNameCombiner CreateDomainNameCombiner(string host)
        {
            if (DomainNameCombiner.TryParseAllLevelSegmentsFromHost(host,
                out IEnumerable<string> allLevelSegments))
            {
                return allLevelSegments.ToList().AsDomainNameCombiner();
            }

            return null;
        }


        /// <summary>
        /// 是指定主机（忽略大小写）。
        /// </summary>
        /// <param name="host">给定的主机。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsHost(string host)
            => Host.Equals(host, StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是指定域名（忽略大小写）。
        /// </summary>
        /// <param name="domainName">给定的 <see cref="DomainNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public bool IsDomainName(DomainNameCombiner domainName)
            => DomainName == domainName;


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="other">给定的域名。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(Uri other)
            => Source.ToString().Equals(other?.ToString(), StringComparison.OrdinalIgnoreCase);

        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="obj">给定的对象。</param>
        /// <returns>返回布尔值。</returns>
        public override bool Equals(object obj)
            => (obj is UriCombiner other) ? Equals(other.Source) : false;


        /// <summary>
        /// 获取哈希码。
        /// </summary>
        /// <returns>返回 32 位整数。</returns>
        public override int GetHashCode()
            => Source.GetHashCode();


        /// <summary>
        /// 转换为字符串。
        /// </summary>
        /// <returns>返回字符串。</returns>
        public override string ToString()
            => Source.ToString();


        /// <summary>
        /// 是否相等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="UriCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="UriCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator ==(UriCombiner a, UriCombiner b)
            => (a?.Equals(b)).Value;

        /// <summary>
        /// 是否不等（忽略大小写）。
        /// </summary>
        /// <param name="a">给定的 <see cref="DomainNameCombiner"/>。</param>
        /// <param name="b">给定的 <see cref="DomainNameCombiner"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool operator !=(UriCombiner a, UriCombiner b)
            => !(a?.Equals(b)).Value;


        /// <summary>
        /// 隐式转换为字符串。
        /// </summary>
        /// <param name="combiner">给定的 <see cref="UriCombiner"/>。</param>
        public static implicit operator string(UriCombiner combiner)
            => combiner?.ToString();


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
            scheme.NotEmpty(nameof(scheme));
            host.NotEmpty(nameof(host));

            return new Uri($"{scheme}{Uri.SchemeDelimiter}{host}{path}{query}{anchor}");
        }


        /// <summary>
        /// 从查询字符串还原查询参数集合（内部支持对参数值的特殊字符进行解码处理）。
        /// </summary>
        /// <param name="queryString">给定的查询参数字符串。</param>
        /// <returns>返回 <see cref="ConcurrentDictionary{String, String}"/>。</returns>
        [SuppressMessage("Microsoft.Design", "CA1062:ValidateArgumentsOfPublicMethods")]
        public static ConcurrentDictionary<string, string> FromQuery(string queryString)
        {
            var queries = new ConcurrentDictionary<string, string>();
            
            if (queryString.IsNotEmpty())
            {
                queryString.TrimStart('?').Split('&').ForEach(segment =>
                {
                    if (segment.IsNotEmpty())
                    {
                        var pair = segment.SplitPair(); // "="
                        var valueString = pair.Value;

                        if (valueString.IsNotEmpty())
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

                if (pair.Value.IsNotEmpty())
                    sb.Append(Uri.EscapeDataString(pair.Value));

                if (i < count - 1)
                    sb.Append("&");
            });

            return Uri.EscapeUriString(sb.ToString());
        }
    }
}
