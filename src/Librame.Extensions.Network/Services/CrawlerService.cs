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

    class CrawlerService : NetworkServiceBase, ICrawlerService
    {
        private readonly IMemoryCache _memoryCache;
        private readonly IServicesManager<IUriRequester, HttpClientRequester> _requesters;


        public CrawlerService(IMemoryCache memoryCache, IServicesManager<IUriRequester, HttpClientRequester> requesters)
            : base(requesters.Default.CastTo<IUriRequester, NetworkServiceBase>(nameof(requesters)))
        {
            _memoryCache = memoryCache.NotNull(nameof(memoryCache));
            _requesters = requesters;

            ImageExtensions = Options.Crawler.ImageExtensions.Split(',');
        }


        public IServicesManager<IUriRequester> Requesters => _requesters;

        public string[] ImageExtensions { get; set; }


        public async Task<IList<string>> GetImageLinksAsync(string url, string pattern = null)
        {
            var hyperLinks = await GetHyperLinksAsync(url, pattern).ConfigureAwait(true);
            Logger.LogDebug($"Get hyper links: {string.Join(",", hyperLinks)}");

            if (hyperLinks.IsNullOrEmpty()) return hyperLinks;

            var imageLinks = hyperLinks.ExtractHasExtension(ImageExtensions).ToList();
            Logger.LogDebug($"Extract images: {string.Join(",", ImageExtensions)}");

            return imageLinks;
        }


        public async Task<IList<string>> GetHyperLinksAsync(string url, string pattern = null)
        {
            var response = await GetContentAsync(url).ConfigureAwait(true);

            var links = new List<string>();

            pattern = pattern.NotEmptyOrDefault(() =>
            {
                return @"(?<url>((http(s)?|ftp|file|ws):)?//([\w-]+\.)+[\w-]+(/[\w- ./?%&=]+)?)|(?<path>(/*[\w- ./?%&=]+\.[\w- .]+)?)";
            });

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Logger.LogDebug($"Create hyper link regex: {pattern}");

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


        public Task<string> GetContentAsync(string url, string postData = null,
            CancellationToken cancellationToken = default)
        {
            var key = $"url:{url}|postData:{postData}";

            return _memoryCache.GetOrCreateAsync(key, entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromSeconds(Options.Crawler.CacheExpirationSeconds));

                return _requesters.Default.GetResponseStringAsync(url, postData,
                    cancellationToken: cancellationToken);
            });
        }

    }
}
