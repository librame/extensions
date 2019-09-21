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

namespace Librame.Extensions.Core
{
    class ClockService : AbstractConcurrentService, IClockService
    {
        public ClockService(IOptions<CoreBuilderOptions> options,
            IMemoryLocker locker, ILoggerFactory loggerFactory)
            : base(locker, loggerFactory)
        {
            Options = options.NotNull(nameof(options)).Value;
        }


        public CoreBuilderOptions Options { get; }


        public Task<DateTime> GetNowAsync(DateTime timestamp, bool? isUtc = null,
            CancellationToken cancellationToken = default)
        {
            return Locker.WaitFactory(() =>
            {
                if (!isUtc.HasValue)
                    isUtc = Options.IsUtcClock;

                return cancellationToken.RunFactoryOrCancellationAsync(() =>
                {
                    var now = isUtc.Value ? DateTime.UtcNow : DateTime.Now;
                    if (timestamp > now)
                    {
                        // 计算时间差并添加补偿以解决时钟回流
                        var offset = (timestamp - now).Add(Options.ClockRefluxOffset);
                        now.Add(offset);

                        Logger.LogWarning($"Suspected clock reflux, check if the local clock is synchronized (IsUTC: {isUtc}).");
                        Logger.LogTrace($"Clock reflux: {timestamp} is greater than {now} (IsUTC: {isUtc}).");
                    }

                    Logger.LogInformation($"Get DateTime: {now.ToString()}");
                    return now;
                });
            });
        }

        public Task<DateTimeOffset> GetOffsetNowAsync(DateTimeOffset timestamp, bool? isUtc = null,
            CancellationToken cancellationToken = default)
        {
            return Locker.WaitFactory(() =>
            {
                if (!isUtc.HasValue)
                    isUtc = Options.IsUtcClock;

                return cancellationToken.RunFactoryOrCancellationAsync(() =>
                {
                    var now = isUtc.Value ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
                    if (timestamp > now)
                    {
                        // 计算时间差并添加补偿以解决时钟回流
                        var offset = (timestamp - now).Add(Options.ClockRefluxOffset);
                        now.Add(offset);

                        Logger.LogWarning("Suspected clock reflux, check if the local clock is synchronized (IsUTC: {isUtc}).");
                        Logger.LogTrace($"Clock reflux: {timestamp} is greater than {now} (IsUTC: {isUtc}).");
                    }

                    Logger.LogInformation($"Get DateTimeOffset: {now.ToString()}");
                    return now;
                });
            });
        }

    }
}
