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

    /// <summary>
    /// 内部时钟服务。
    /// </summary>
    internal class InternalClockService : AbstractService, IClockService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalClockService"/> 实例。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public InternalClockService(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        /// <summary>
        /// 异步获取当前协调世界时(UTC)的日期和时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
        public Task<DateTime> GetNowAsync(CancellationToken cancellationToken = default)
        {
            return cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var now = DateTime.Now;
                Logger.LogInformation($"Get DateTime: {now.ToString()}");

                return now;
            });
        }

        /// <summary>
        /// 异步获取当前协调世界时(UTC)的日期和时间。
        /// </summary>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
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
