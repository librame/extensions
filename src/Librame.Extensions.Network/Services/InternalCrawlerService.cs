#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 内部抓取器服务。
    /// </summary>
    internal class InternalCrawlerService : NetworkServiceBase, ICrawlerService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IServicesManager<IUriRequester, InternalHttpClientRequester> _requesters;


        /// <summary>
        /// 构造一个 <see cref="InternalCrawlerService"/> 实例。
        /// </summary>
        /// <param name="memoryCache">给定的 <see cref="IMemoryCache"/>。</param>
        /// <param name="requesters">给定的 <see cref="IServicesManager{IUriRequester, InternalHttpClientRequester}"/>。</param>
        public InternalCrawlerService(IMemoryCache memoryCache, IServicesManager<IUriRequester, InternalHttpClientRequester> requesters)
            : base(requesters.Defaulter.CastTo<IUriRequester, NetworkServiceBase>(nameof(requesters)))
        {
            _memoryCache = memoryCache.NotNull(nameof(memoryCache));
            _requesters = requesters;

            ImageExtensions = Options.Crawler.ImageExtensions.Split(',');
        }


        /// <summary>
        /// 请求程序管理器。
        /// </summary>
        public IServicesManager<IUriRequester> Requesters => _requesters;

        /// <summary>
        /// 图像文件扩展名集合。
        /// </summary>
        public string[] ImageExtensions { get; set; }


        /// <summary>
        /// 获取链接响应内容中包含的所有图像类超链接。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="pattern">给定包含 url 与 path 分组名的超链接正则表达式匹配模式（可选）。</param>
        /// <returns>返回一个包含图像类超链接列表的异步操作。</returns>
        public async Task<IList<string>> GetImageLinksAsync(string url, string pattern = null)
        {
            var hyperLinks = await GetHyperLinksAsync(url, pattern);
            Logger.LogDebug($"Get hyper links: {string.Join(",", hyperLinks)}");

            if (hyperLinks.IsNullOrEmpty()) return hyperLinks;

            var imageLinks = hyperLinks.ExtractHasExtension(ImageExtensions).ToList();
            Logger.LogDebug($"Extract images: {string.Join(",", ImageExtensions)}");

            return imageLinks;
        }


        /// <summary>
        /// 获取链接响应内容中包含的所有超链接。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="pattern">给定包含 url 与 path 分组名的超链接正则表达式匹配模式（可选）。</param>
        /// <returns>返回一个包含超链接列表的异步操作。</returns>
        public async Task<IList<string>> GetHyperLinksAsync(string url, string pattern = null)
        {
            pattern = pattern.EnsureString(() =>
            {
                return @"(?<url>((http(s)?|ftp|file|ws):)?//([\w-]+\.)+[\w-]+(/[\w- ./?%&=]+)?)|(?<path>(/*[\w- ./?%&=]+\.[\w- .]+)?)";
            });

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Logger.LogDebug($"Create hyper link regex: {pattern}");

            var links = new List<string>();

            var response = await GetContentAsync(url);
            var matches = regex.Matches(response);
            if (matches.Count < 1)
                return links;
            
            Logger.LogDebug($"Match count: {matches.Count}");

            for (int i = 0; i <= matches.Count - 1; i++)
            {
                var groups = matches[i].Groups;
                var urlGroup = groups["url"];
                var pathGroup = groups["path"];

                if (!urlGroup.Value.IsNullOrEmpty())
                {
                    // URL 链接
                    var link = urlGroup.Value;
                    Logger.LogDebug($"Match url: {link}");

                    // 修正无方案的情况（如：//domain.com/file.ext）
                    if (link.StartsWith("//"))
                    {
                        var schemeIndex = url.IndexOf("//");
                        if (schemeIndex < 1)
                        {
                            // 如果方案提取失败，则跳过此链接（因本地文件不能提取 URL 方案）
                            continue;
                        }

                        var scheme = url.Substring(0, schemeIndex);
                        link = scheme + link;
                    }

                    var linkUrl = new Uri(link);
                    if (!linkUrl.PathAndQuery.IsNullOrEmpty() && linkUrl.PathAndQuery != "/")
                        links.Add(link);
                }
                else if (!pathGroup.Value.IsNullOrEmpty())
                {
                    // 虚拟路径
                    var link = pathGroup.Value;
                    Logger.LogDebug($"Match path: {link}");

                    // 排除代码中的注释情况（如：// console.log）
                    if (!link.StartsWith("//"))
                    {
                        var baseUrl = new Uri(url);
                        link = new Uri(baseUrl, link).ToString();

                        links.Add(link);
                    }
                }
                else
                {
                    //
                }
            }
            
            return links.Distinct().ToList();
        }


        /// <summary>
        /// 异步获取链接响应内容。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="postData">给定用于提交请求的数据（可选；默认不提交数据）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含响应内容的异步操作。</returns>
        public Task<string> GetContentAsync(string url, string postData = null,
            CancellationToken cancellationToken = default)
        {
            var key = $"url:{url}|postData:{postData}";

            return _memoryCache.GetOrCreateAsync(key, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(Options.Crawler.CacheExpirationSeconds));

                return _requesters.Defaulter.GetResponseStringAsync(url, postData,
                    cancellationToken: cancellationToken);
            });
        }

    }
}
