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
    /// 内部 HTTP Web 请求工厂。
    /// </summary>
    internal class InternalHttpWebRequestFactory : AbstractRequestFactory<HttpWebRequest>, IRequestFactory<HttpWebRequest>
    {
        /// <summary>
        /// 构造一个 <see cref="InternalHttpWebRequestFactory"/> 实例。
        /// </summary>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        public InternalHttpWebRequestFactory(IOptions<NetworkBuilderOptions> options,
            IOptions<CoreBuilderOptions> coreOptions)
            : base(options, coreOptions)
        {
        }


        /// <summary>
        /// 创建请求。
        /// </summary>
        /// <param name="url">给定的 URL。</param>
        /// <param name="method">给定的请求方法（可选；默认 POST）。</param>
        /// <returns>返回 <see cref="HttpWebRequest"/>。</returns>
        public override HttpWebRequest CreateRequest(string url, string method = "POST")
        {
            var opts = Options.Request;
            
            var hwr = WebRequest.CreateHttp(url);
            hwr.AllowAutoRedirect = opts.AllowAutoRedirect;
            hwr.Referer = opts.Referer;
            hwr.Timeout = opts.Timeout;
            hwr.UserAgent = opts.UserAgent;
            hwr.Method = method;

            if (method.Equals("post", StringComparison.OrdinalIgnoreCase))
                hwr.ContentType = $"application/x-www-form-urlencoded; charset={Encoding.AsName()}";

            return hwr;
        }

    }
}
