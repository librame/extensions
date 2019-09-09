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
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data
{
    using Core;

    class ClockService : AbstractService, IClockService
    {
        private readonly DataBuilderOptions _options;


        public ClockService(IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            _options = options.Value;
        }


        public Task<DateTime> GetNowAsync(bool? isUtc = null, CancellationToken cancellationToken = default)
        {
            if (!isUtc.HasValue)
                isUtc = _options.IsUtcClock;

            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var now = isUtc.Value ? DateTime.UtcNow : DateTime.Now;
                Logger.LogInformation($"Get DateTime: {now.ToString()}");

                return now;
            });
        }

        public Task<DateTimeOffset> GetOffsetNowAsync(bool? isUtc = null, CancellationToken cancellationToken = default)
        {
            if (!isUtc.HasValue)
                isUtc = _options.IsUtcClock;

            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var now = isUtc.Value ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
                Logger.LogInformation($"Get DateTimeOffset: {now.ToString()}");

                return now;
            });
        }

    }
}
