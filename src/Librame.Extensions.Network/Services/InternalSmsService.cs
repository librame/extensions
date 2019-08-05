#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

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
        private readonly IServicesManager<IUriRequester, HttpClientRequester> _requesters;


        /// <summary>
        /// 构造一个 <see cref="InternalSmsService"/> 实例。
        /// </summary>
        /// <param name="requesters">给定的 <see cref="IServicesManager{IUriRequester, InternalHttpClientRequester}"/>。</param>
        public InternalSmsService(IServicesManager<IUriRequester, HttpClientRequester> requesters)
            : base(requesters.Defaulter.CastTo<IUriRequester, NetworkServiceBase>(nameof(requesters)))
        {
            _requesters = requesters;
        }


        /// <summary>
        /// 请求程序管理器。
        /// </summary>
        public IServicesManager<IUriRequester> Requesters => _requesters;


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

            var parameters = new RequestParameters
            {
                Accept = "text/xml,text/javascript"
            };

            return _requesters.Defaulter.GetResponseStringAsync(gatewayUrl, message,
                Options.Sms.EnableCodec, parameters, cancellationToken);
        }

    }
}
