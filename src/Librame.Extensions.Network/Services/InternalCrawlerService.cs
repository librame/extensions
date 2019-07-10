#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
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
        private readonly IRequestFactory<HttpWebRequest> _requestFactory;


        /// <summary>
        /// 构造一个 <see cref="InternalCrawlerService"/> 实例。
        /// </summary>
        /// <param name="requestFactory">给定的 <see cref="IRequestFactory{HttpWebRequest}"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalCrawlerService(IRequestFactory<HttpWebRequest> requestFactory,
            IOptions<CoreBuilderOptions> coreOptions,
            IOptions<NetworkBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(coreOptions, options, loggerFactory)
        {
            _requestFactory = requestFactory.NotNull(nameof(requestFactory));
            ImageExtensions = Options.Crawler.ImageExtensions.Split(',');
        }

        
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
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string response = null;

                using (var s = CreateResponse(url, postData).GetResponseStream())
                {
                    if (s.IsNotNull())
                    {
                        using (var sr = new StreamReader(s, Encoding))
                        {
                            response = sr.ReadToEnd();
                            Logger.LogDebug($"Response: {response}");
                        }
                    }
                }

                return response;
            });
        }

        private WebResponse CreateResponse(string url, string postData = null)
        {
            try
            {
                var method = postData.IsNullOrEmpty() ? "GET" : "POST";
                var hwr = _requestFactory.CreateRequest(url, method);

                if (!postData.IsNullOrEmpty())
                {
                    var buffer = Encoding.GetBytes(postData);
                    hwr.ContentLength = buffer.Length;

                    using (var s = hwr.GetRequestStream())
                    {
                        s.Write(buffer, 0, buffer.Length);
                        Logger.LogDebug($"Send string: {postData}");
                    }
                }

                return hwr.GetResponse();
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
                throw ex;
            }
        }

    }
}
