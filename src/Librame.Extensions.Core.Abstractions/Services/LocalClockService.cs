#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core.Services
{
    /// <summary>
    /// 本地时钟服务。
    /// </summary>
    public class LocalClockService : AbstractService, IClockService
    {
        /// <summary>
        /// 默认实例。
        /// </summary>
        public static readonly IClockService Default
            = new LocalClockService();


        private readonly TimeSpan _clockRefluxOffsetSeconds;


        private LocalClockService()
            : base()
        {
            _clockRefluxOffsetSeconds = TimeSpan.FromSeconds(1);
        }


        /// <summary>
        /// 获取当前日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选；通常为当前时间，可解决时间回流）。</param>
        /// <param name="isUtc">相对于协调世界时（可选；默认使用选项设置）。</param>
        /// <returns>返回 <see cref="DateTime"/>。</returns>
        public DateTime GetNow(DateTime? timestamp = null, bool? isUtc = null)
        {
            if (!isUtc.HasValue)
                isUtc = true;

            var localNow = isUtc.Value ? DateTime.UtcNow : DateTime.Now;

            if (timestamp.HasValue && timestamp.Value > localNow)
            {
                // 计算时间差并添加补偿以解决时钟回流
                var offset = (timestamp.Value - localNow).Add(_clockRefluxOffsetSeconds);
                localNow.Add(offset);
            }

            return localNow;
        }

        /// <summary>
        /// 异步获取当前日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选；通常为当前时间，可解决时间回流）。</param>
        /// <param name="isUtc">相对于协调世界时（可选；默认使用选项设置）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTime"/> 的异步操作。</returns>
        public Task<DateTime> GetNowAsync(DateTime? timestamp = null, bool? isUtc = null,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelAsync(() => GetNow(timestamp, isUtc));


        /// <summary>
        /// 获取相对于协调世界时(UTC)的日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选；通常为当前时间，可解决时间回流）。</param>
        /// <param name="isUtc">相对于协调世界时（可选；默认使用选项设置）。</param>
        /// <returns>返回一个包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
        public DateTimeOffset GetNowOffset(DateTimeOffset? timestamp = null, bool? isUtc = null)
        {
            if (!isUtc.HasValue)
                isUtc = true;

            var localNow = isUtc.Value ? DateTimeOffset.UtcNow : DateTimeOffset.Now;

            if (timestamp.HasValue && timestamp.Value > localNow)
            {
                // 计算时间差并添加补偿以解决时钟回流
                var offset = (timestamp.Value - localNow).Add(_clockRefluxOffsetSeconds);
                localNow.Add(offset);
            }

            return localNow;
        }

        /// <summary>
        /// 异步获取相对于协调世界时(UTC)的日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选；通常为当前时间，可解决时间回流）。</param>
        /// <param name="isUtc">相对于协调世界时（可选；默认使用选项设置）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
        public Task<DateTimeOffset> GetNowOffsetAsync(DateTimeOffset? timestamp = null, bool? isUtc = null,
            CancellationToken cancellationToken = default)
            => cancellationToken.RunOrCancelAsync(() => GetNowOffset(timestamp, isUtc));

    }
}
