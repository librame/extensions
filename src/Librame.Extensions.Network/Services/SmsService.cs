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

    class SmsService : NetworkServiceBase, ISmsService
    {
        private readonly IServicesManager<IUriRequester, HttpClientRequester> _requesters;


        public SmsService(IServicesManager<IUriRequester, HttpClientRequester> requesters)
            : base(requesters.Defaulter.CastTo<IUriRequester, NetworkServiceBase>(nameof(requesters)))
        {
            _requesters = requesters;
        }


        public IServicesManager<IUriRequester> Requesters => _requesters;


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
