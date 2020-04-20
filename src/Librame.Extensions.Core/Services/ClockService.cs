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
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Services
{
    using Builders;
    using Threads;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ClockService : AbstractConcurrentService, IClockService
    {
        private readonly TimeSpan _clockRefluxOffsetSeconds;


        public ClockService(IOptions<CoreBuilderOptions> options,
            IMemoryLocker locker, ILoggerFactory loggerFactory)
            : base(locker, loggerFactory)
        {
            Options = options.NotNull(nameof(options)).Value;
            _clockRefluxOffsetSeconds = TimeSpan.FromSeconds(Options.ClockRefluxOffset);
        }


        public CoreBuilderOptions Options { get; }


        public DateTime GetNow(DateTime timestamp, bool? isUtc = null)
            => Locker.WaitFactory(() => GetNowCore(timestamp, isUtc));

        public Task<DateTime> GetNowAsync(DateTime timestamp, bool? isUtc = null,
            CancellationToken cancellationToken = default)
        {
            return Locker.WaitFactoryAsync(() =>
            {
                return cancellationToken.RunFactoryOrCancellationAsync(()
                    => GetNowCore(timestamp, isUtc));
            });
        }


        public DateTimeOffset GetOffsetNow(DateTimeOffset timestamp, bool? isUtc = null)
            => Locker.WaitFactory(() => GetOffsetNowCore(timestamp, isUtc));

        public Task<DateTimeOffset> GetOffsetNowAsync(DateTimeOffset timestamp, bool? isUtc = null,
            CancellationToken cancellationToken = default)
        {
            return Locker.WaitFactoryAsync(() =>
            {
                return cancellationToken.RunFactoryOrCancellationAsync(()
                    => GetOffsetNowCore(timestamp, isUtc));
            });
        }


        private DateTime GetNowCore(DateTime timestamp, bool? isUtc = null)
        {
            if (!isUtc.HasValue)
                isUtc = Options.IsUtcClock;

            var now = isUtc.Value ? DateTime.UtcNow : DateTime.Now;
            if (timestamp > now)
            {
                // 计算时间差并添加补偿以解决时钟回流
                var offset = (timestamp - now).Add(_clockRefluxOffsetSeconds);
                now.Add(offset);

                Logger.LogWarning($"Suspected clock reflux, check if the local clock is synchronized (IsUTC: {isUtc}).");
                Logger.LogTrace($"Clock reflux: {timestamp} is greater than {now} (IsUTC: {isUtc}).");
            }

            Logger.LogInformation($"Get DateTime: {now.ToString(CultureInfo.CurrentCulture)}");
            return now;
        }

        private DateTimeOffset GetOffsetNowCore(DateTimeOffset timestamp, bool? isUtc = null)
        {
            if (!isUtc.HasValue)
                isUtc = Options.IsUtcClock;

            var now = isUtc.Value ? DateTimeOffset.UtcNow : DateTimeOffset.Now;
            if (timestamp > now)
            {
                // 计算时间差并添加补偿以解决时钟回流
                var offset = (timestamp - now).Add(_clockRefluxOffsetSeconds);
                now.Add(offset);

                Logger.LogWarning($"Suspected clock reflux, check if the local clock is synchronized (IsUTC: {isUtc}).");
                Logger.LogTrace($"Clock reflux: {timestamp} is greater than {now} (IsUTC: {isUtc}).");
            }

            Logger.LogInformation($"Get DateTimeOffset: {now.ToString(CultureInfo.CurrentCulture)}");
            return now;
        }

    }
}
