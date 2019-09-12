﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Core
{
    /// <summary>
    /// 时钟服务接口。
    /// </summary>
    public interface IClockService : IService
    {
        /// <summary>
        /// 异步获取当前日期和时间。
        /// </summary>
        /// <param name="isUtc">相对于协调世界时（可选；默认使用选项设置）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
        Task<DateTime> GetNowAsync(bool? isUtc = null, CancellationToken cancellationToken = default);

        /// <summary>
        /// 异步获取相对于协调世界时(UTC)的日期和时间。
        /// </summary>
        /// <param name="isUtc">相对于协调世界时（可选；默认使用选项设置）。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="DateTimeOffset"/> 的异步操作。</returns>
        Task<DateTimeOffset> GetOffsetNowAsync(bool? isUtc = null, CancellationToken cancellationToken = default);
    }
}