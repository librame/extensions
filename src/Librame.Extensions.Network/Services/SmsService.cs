#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    using Core;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class SmsService : NetworkServiceBase, ISmsService
    {
        private readonly IServicesManager<IUriRequester, HttpClientRequester> _requesters;


        public SmsService(IServicesManager<IUriRequester, HttpClientRequester> requesters)
            : base(requesters.DefaultService.CastTo<IUriRequester, NetworkServiceBase>(nameof(requesters)))
        {
            _requesters = requesters;
        }


        public async Task<string> SendAsync(string mobile, string text)
        {
            var list = await SendAsync(new ShortMessageDescriptor(mobile, text)).ConfigureAndResultAsync();
            return list.First();
        }

        public async Task<string[]> SendAsync(params ShortMessageDescriptor[] descriptors)
        {
            var parameters = new RequestParameters
            {
                Accept = "text/xml,text/javascript"
            };

            var list = new List<string>();

            foreach (var descr in descriptors)
            {
                var gatewayUrl = Options.Sms.GetewayUrlFactory.Invoke(Options.Sms.PlatformInfo, descr);

                var result = await _requesters.DefaultService.GetResponseStringAsync(gatewayUrl, postData: null,
                    Options.Sms.EnableCodec, parameters).ConfigureAndResultAsync();

                list.Add(result);
            }

            return list.ToArray();
        }

    }
}
