﻿#region License

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

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;
    using Data.Builders;

    /// <summary>
    /// <see cref="Guid"/> 数据存储标识符生成器。
    /// </summary>
    public class GuidDataStoreIdentifierGenerator : AbstractDataStoreIdentifierGenerator<Guid>
    {
        /// <summary>
        /// 构造一个 <see cref="GuidDataStoreIdentifierGenerator"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public GuidDataStoreIdentifierGenerator(IClockService clock,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(clock, options?.Value.IdentifierGenerator, loggerFactory)
        {
        }

    }
}