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

namespace Librame.Extensions.Data
{
    using Core;

    class ClockService : AbstractService, IClockService
    {
        public ClockService(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        public Task<DateTime> GetNowAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var now = DateTime.Now;
                Logger.LogInformation($"Get DateTime: {now.ToString()}");

                return now;
            });
        }

        public Task<DateTimeOffset> GetUtcNowAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var now = DateTimeOffset.Now;
                Logger.LogInformation($"Get UTC DateTime: {now.ToString()}");

                return now;
            });
        }

    }
}
