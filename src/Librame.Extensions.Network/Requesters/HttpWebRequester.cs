#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Options;
using System;
using System.Net;

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// HTTP WEB 请求程序。
    /// </summary>
    public class HttpWebRequester : AbstractRequester, IHttpRequester<HttpWebRequest>
    {
        /// <summary>
        /// 构造一个 <see cref="HttpWebRequester"/> 实例。
        /// </summary>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        public HttpWebRequester(IOptions<NetworkBuilderOptions> options,
            IOptions<CoreBuilderOptions> coreOptions)
            : base(options, coreOptions)
        {
        }


        /// <summary>
        /// 创建请求程序。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="method">给定的请求方法（可选；默认 POST）。</param>
        /// <returns>返回 <see cref="HttpWebRequest"/>。</returns>
        public HttpWebRequest CreateRequest(string url, string method = "POST")
        {
            var requesterOptions = CoreOptions.Requester;
            
            var hwr = WebRequest.CreateHttp(url);
            Logger.LogDebug($"Create http web request: {url}");

            hwr.AllowAutoRedirect = requesterOptions.AllowAutoRedirect;
            Logger.LogDebug($"Set allow auto redirect: {hwr.AllowAutoRedirect}");

            hwr.Referer = requesterOptions.Referer;
            Logger.LogDebug($"Set referer: {hwr.Referer}");

            hwr.Timeout = requesterOptions.Timeout;
            Logger.LogDebug($"Set timeout: {hwr.Timeout}");

            hwr.UserAgent = requesterOptions.UserAgent;
            Logger.LogDebug($"Set user agent: {hwr.UserAgent}");

            hwr.Method = method;
            Logger.LogDebug($"Set method: {hwr.Method}");

            if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
            {
                hwr.ContentType = $"application/x-www-form-urlencoded; charset={charset}";
                Logger.LogDebug($"Set content type: {hwr.ContentType}");
            }

            return hwr;
        }

    }
}
