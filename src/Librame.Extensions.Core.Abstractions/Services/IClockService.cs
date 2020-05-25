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
    /// 时钟服务接口。
    /// </summary>
    public interface IClockService : IService
    {
        /// <summary>
        /// 获取当前日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选；通常为当前时间，可解决时间回流）。</param>
        /// <param name="isUtc">相对于协调世界时（可选；默认使用选项设置）。</param>
        /// <returns>返回 <see cref="DateTime"/>。</returns>
        DateTime GetNow(DateTime? timestamp = null, bool? isUtc = null);

        /// <summary>
        /// 异步获取当前日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选；通常为当前时间，可解决时间回流）。</param>
        /// <param name="isUtc">相对于协调世界时（可选；默认使用选项设置）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTime"/> 的异步操作。</returns>
        Task<DateTime> GetNowAsync(DateTime? timestamp = null, bool? isUtc = null,
            CancellationToken cancellationToken = default);


        /// <summary>
        /// 获取相对于当前协调世界时(UTC)的日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选；通常为当前时间，可解决时间回流）。</param>
        /// <param name="isUtc">相对于协调世界时（可选；默认使用选项设置）。</param>
        /// <returns>返回一个包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
        DateTimeOffset GetNowOffset(DateTimeOffset? timestamp = null, bool? isUtc = null);

        /// <summary>
        /// 异步获取相对于当前协调世界时(UTC)的日期和时间。
        /// </summary>
        /// <param name="timestamp">给定的时间戳（可选；通常为当前时间，可解决时间回流）。</param>
        /// <param name="isUtc">相对于协调世界时（可选；默认使用选项设置）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
        Task<DateTimeOffset> GetNowOffsetAsync(DateTimeOffset? timestamp = null, bool? isUtc = null,
            CancellationToken cancellationToken = default);
    }
}
