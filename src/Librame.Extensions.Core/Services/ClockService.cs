#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
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

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class ClockService : AbstractService, IClockService
    {
        private readonly TimeSpan _clockRefluxOffsetSeconds;


        public ClockService(IOptions<CoreBuilderOptions> options,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Options = options.NotNull(nameof(options)).Value;
            _clockRefluxOffsetSeconds = TimeSpan.FromSeconds(Options.ClockRefluxOffset);
        }


        public CoreBuilderOptions Options { get; }


        public DateTime GetNow(DateTime? timestamp = null, bool? isUtc = null)
        {
            if (!isUtc.HasValue)
                isUtc = Options.IsUtcClock;

            var localNow = isUtc.Value ? DateTime.UtcNow : DateTime.Now;

            if (timestamp.HasValue && timestamp.Value > localNow)
            {
                // 计算时间差并添加补偿以解决时钟回流
                var offset = (timestamp.Value - localNow).Add(_clockRefluxOffsetSeconds);
                localNow.Add(offset);

                Logger.LogWarning($"Suspected clock reflux, check if the local clock is synchronized (IsUTC: {isUtc}).");
                Logger.LogTrace($"Clock reflux: {timestamp} is greater than {localNow} (IsUTC: {isUtc}).");
            }

            Logger.LogInformation($"Get DateTime: {localNow.ToString(CultureInfo.CurrentCulture)}");
            return localNow;
        }

        public Task<DateTime> GetNowAsync(DateTime? timestamp = null, bool? isUtc = null,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => GetNow(timestamp, isUtc));


        public DateTimeOffset GetNowOffset(DateTimeOffset? timestamp = null, bool? isUtc = null)
        {
            if (!isUtc.HasValue)
                isUtc = Options.IsUtcClock;

            var localNow = isUtc.Value ? DateTimeOffset.UtcNow : DateTimeOffset.Now;

            if (timestamp.HasValue && timestamp.Value > localNow)
            {
                // 计算时间差并添加补偿以解决时钟回流
                var offset = (timestamp.Value - localNow).Add(_clockRefluxOffsetSeconds);
                localNow.Add(offset);

                Logger.LogWarning($"Suspected clock reflux, check if the local clock is synchronized (IsUTC: {isUtc}).");
                Logger.LogTrace($"Clock reflux: {timestamp} is greater than {localNow} (IsUTC: {isUtc}).");
            }

            Logger.LogInformation($"Get DateTimeOffset: {localNow.ToString(CultureInfo.CurrentCulture)}");
            return localNow;
        }

        public Task<DateTimeOffset> GetNowOffsetAsync(DateTimeOffset? timestamp = null, bool? isUtc = null,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() => GetNowOffset(timestamp, isUtc));

    }
}
