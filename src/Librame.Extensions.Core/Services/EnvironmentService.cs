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
    class EnvironmentService : AbstractConcurrentService, IEnvironmentService
    {
        public EnvironmentService(IMemoryLocker locker, ILoggerFactory loggerFactory)
            : base(locker, loggerFactory)
        {
        }


        public Task<IEnvironmentInfo> GetEnvironmentInfoAsync(CancellationToken cancellationToken = default)
        {
            return Locker.WaitFactory(() =>
            {
                return cancellationToken.RunFactoryOrCancellationAsync(() =>
                {
                    IEnvironmentInfo info = new EnvironmentInfo();
                    Logger.LogInformation($"Refresh environment info at {DateTimeOffset.Now.ToString()}");

                    return info;
                });
            });
        }

    }
}
