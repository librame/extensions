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
using System.IO;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 内部短信服务。
    /// </summary>
    internal class InternalSmsService : NetworkServiceBase, ISmsService
    {
        private readonly IRequestFactory<HttpWebRequest> _requestFactory;
        private readonly IByteCodecService _byteCodec;


        /// <summary>
        /// 构造一个 <see cref="InternalSmsService"/> 实例。
        /// </summary>
        /// <param name="requestFactory">给定的 <see cref="IRequestFactory{HttpWebRequest}"/>。</param>
        /// <param name="byteCodec">给定的 <see cref="IByteCodecService"/>。</param>
        /// <param name="coreOptions">给定的 <see cref="IOptions{CoreBuilderOptions}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalSmsService(IRequestFactory<HttpWebRequest> requestFactory,
            IByteCodecService byteCodec, IOptions<CoreBuilderOptions> coreOptions,
            IOptions<NetworkBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(coreOptions, options, loggerFactory)
        {
            _requestFactory = requestFactory.NotNull(nameof(requestFactory));
            _byteCodec = byteCodec.NotNull(nameof(byteCodec));
        }


        /// <summary>
        /// 异步发送。
        /// </summary>
        /// <param name="message">给定要发送的消息。</param>
        /// <param name="gatewayUrl">给定的短信网关 URL（可选；默认为选项设定）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含响应内容的异步操作。</returns>
        public Task<string> SendAsync(string message, string gatewayUrl = null,
            CancellationToken cancellationToken = default)
        {
            if (gatewayUrl.IsNullOrEmpty())
                gatewayUrl = Options.Sms.GetewayUrl;

            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                string response = null;

                using (var s = CreateResponse(gatewayUrl, message).GetResponseStream())
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

        private WebResponse CreateResponse(string url, string message)
        {
            try
            {
                var hwr = _requestFactory.CreateRequest(url);
                hwr.Accept = "text/xml,text/javascript";
                hwr.ContinueTimeout = Options.Sms.ContinueTimeout;

                var buffer = _byteCodec.EncodeStringAsBytes(message, Options.Sms.EnableCodec);
                hwr.ContentLength = buffer.Length;

                using (var s = hwr.GetRequestStream())
                {
                    s.Write(buffer, 0, buffer.Length);
                    Logger.LogDebug($"Send string: {message}");
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
