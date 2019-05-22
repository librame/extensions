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
using System.Text;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    using Encryption;

    /// <summary>
    /// 内部短信服务。
    /// </summary>
    internal class InternalSmsService : AbstractNetworkService<InternalSmsService>, ISmsService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalSmsService"/> 实例。
        /// </summary>
        /// <param name="hash">给定的 <see cref="IHashService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalSmsService}"/>。</param>
        public InternalSmsService(IHashService hash,
            IOptions<NetworkBuilderOptions> options, ILogger<InternalSmsService> logger)
            : base(hash, options, logger)
        {
        }


        /// <summary>
        /// 异步发送。
        /// </summary>
        /// <param name="message">给定要发送的消息。</param>
        /// <param name="gatewayUrlFactory">给定的短信网关选项设定工厂方法。</param>
        /// <returns>返回响应的字符串。</returns>
        public async Task<string> SendAsync(string message, Func<string, Uri> gatewayUrlFactory)
        {
            var url = gatewayUrlFactory.Invoke(Options.Sms.GetewayUrl);
            
            return await SendCoreAsync(message, url);
        }

        /// <summary>
        /// 异步发送。
        /// </summary>
        /// <param name="message">给定要发送的消息。</param>
        /// <param name="gatewayUrl">给定的短信网关 URL（可选；默认为选项设定）。</param>
        /// <returns>返回响应的字符串。</returns>
        public async Task<string> SendAsync(string message, string gatewayUrl = null)
        {
            var url = new Uri(gatewayUrl ?? Options.Sms.GetewayUrl);

            return await SendCoreAsync(message, url);
        }


        private async Task<string> SendCoreAsync(string message, Uri url)
        {
            var result = string.Empty;

            try
            {
                var hwr = WebRequest.CreateHttp(url);
                Logger.LogDebug($"Create http web request for gateway: {url.ToString()}");

                hwr.ContentType = $"application/x-www-form-urlencoded;charset={Encoding.AsName()}";
                Logger.LogDebug($"Set content type: {hwr.ContentType}");

                hwr.Method = "POST";
                Logger.LogDebug($"Set method: {hwr.Method}");

                hwr.Accept = "text/xml,text/javascript";
                Logger.LogDebug($"Set accept: {hwr.Accept}");

                hwr.ContinueTimeout = Options.Sms.ContinueTimeout;
                Logger.LogDebug($"Set continue timeout: {hwr.ContinueTimeout}");

                using (var s = await hwr.GetRequestStreamAsync())
                {
                    var bytes = ReadySendData(message);
                    
                    s.Write(bytes, 0, bytes.Length);
                    Logger.LogDebug($"Send message: {message}");
                }

                using (var r = (HttpWebResponse)await hwr.GetResponseAsync())
                {
                    // 以字符流的方式读取 HTTP 响应
                    using (var rs = r.GetResponseStream())
                    {
                        var sr = new StreamReader(rs, Encoding);
                        result = sr.ReadToEnd();
                        Logger.LogDebug($"Receive message: {result}");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }

            return result;
        }

    }
}
