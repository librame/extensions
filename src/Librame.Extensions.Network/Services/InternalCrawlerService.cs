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
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    using Encryption;

    /// <summary>
    /// 内部抓取器服务。
    /// </summary>
    internal class InternalCrawlerService : AbstractNetworkService<InternalCrawlerService>, ICrawlerService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalCrawlerService"/> 实例。
        /// </summary>
        /// <param name="hash">给定的 <see cref="IHashService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DefaultNetworkBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalCrawlerService}"/>。</param>
        public InternalCrawlerService(IHashService hash,
            IOptions<NetworkBuilderOptions> options, ILogger<InternalCrawlerService> logger)
            : base(hash, options, logger)
        {
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
        public async Task<IList<string>> GetImageHyperLinks(string url, string pattern = null)
        {
            var hyperLinks = await GetHyperLinks(url, pattern);
            Logger.LogDebug($"Get hyper links: {string.Join(",", hyperLinks)}");

            if (hyperLinks.IsNullOrEmpty()) return hyperLinks;

            var list = hyperLinks.ExtractHasExtension(ImageExtensions).ToList();
            Logger.LogDebug($"Extract images: {string.Join(",", ImageExtensions)}");

            return list;
        }


        /// <summary>
        /// 获取链接响应内容中包含的所有超链接。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="pattern">给定包含 url 与 path 分组名的超链接正则表达式匹配模式（可选）。</param>
        /// <returns>返回一个包含超链接列表的异步操作。</returns>
        public async Task<IList<string>> GetHyperLinks(string url, string pattern = null)
        {
            pattern = pattern.HasOrDefault(() =>
            {
                return @"(?<url>((http(s)?|ftp|file|ws):)?//([\w-]+\.)+[\w-]+(/[\w- ./?%&=]+)?)|(?<path>(/*[\w- ./?%&=]+\.[\w- .]+)?)";
            });

            var regex = new Regex(pattern, RegexOptions.IgnoreCase);
            Logger.LogDebug($"Create hyper link regex: {pattern}");

            var links = new List<string>();

            var htmlCode = await GetString(url);
            var matches = regex.Matches(htmlCode);

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
        /// 获取链接响应的内容。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <returns>返回一个包含响应内容的异步操作。</returns>
        public Task<string> GetString(string url)
        {
            return GetResponseString(url);
        }


        /// <summary>
        /// 提交链接响应的内容。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="postData">给定用于提交请求的数据。</param>
        /// <returns>返回一个包含响应内容的异步操作。</returns>
        public Task<string> PostString(string url, string postData)
        {
            return GetResponseString(url, postData);
        }


        /// <summary>
        /// 获取链接响应内容。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="postData">给定用于提交请求的数据。</param>
        /// <returns>返回一个包含响应内容的异步操作。</returns>
        protected Task<string> GetResponseString(string url, string postData = null)
        {
            string result = null;

            using (var s = CreateResponse(url, postData).GetResponseStream())
            {
                if (s.IsNotNull())
                {
                    using (var sr = new StreamReader(s))
                    {
                        result = sr.ReadToEnd();
                        Logger.LogDebug($"Receive string: {result}");
                    }
                }
            }

            return Task.FromResult(result);
        }

        /// <summary>
        /// 创建链接响应。
        /// </summary>
        /// <param name="url">给定的 URL 链接。</param>
        /// <param name="postData">给定用于提交请求的数据。</param>
        /// <returns>返回 <see cref="WebResponse"/>。</returns>
        protected WebResponse CreateResponse(string url, string postData = null)
        {
            try
            {
                var hwr = WebRequest.CreateHttp(url);
                Logger.LogDebug($"Create http web request: {url}");

                hwr.AllowAutoRedirect = Options.Crawler.AllowAutoRedirect;
                Logger.LogDebug($"Set allow auto redirect: {hwr.AllowAutoRedirect}");

                hwr.Referer = Options.Crawler.Referer;
                Logger.LogDebug($"Set referer: {hwr.Referer}");

                hwr.Timeout = Options.Crawler.Timeout;
                Logger.LogDebug($"Set timeout: {hwr.Timeout}");

                hwr.UserAgent = Options.Crawler.UserAgent;
                Logger.LogDebug($"Set user agent: {hwr.UserAgent}");

                if (!postData.IsNullOrEmpty())
                {
                    var buffer = Encoding.GetBytes(postData);

                    hwr.Method = "POST";
                    Logger.LogDebug($"Set method: {hwr.Method}");

                    hwr.ContentType = "application/x-www-form-urlencoded;charset=utf-8";
                    Logger.LogDebug($"Set content type: {hwr.ContentType}");

                    hwr.ContentLength = buffer.Length;
                    Logger.LogDebug($"Set content length: {hwr.ContentLength}");

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
