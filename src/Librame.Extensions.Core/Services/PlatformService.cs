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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    class PlatformService : AbstractService, IPlatformService
    {
        public PlatformService(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public Task<IEnvironmentInfo> GetEnvironmentInfoAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                IEnvironmentInfo info = new ApplicationEnvironmentInfo();
                Logger.LogInformation($"Refresh environment info at {DateTimeOffset.Now.ToString()}");

                return info;
            });
        }

    }
}
