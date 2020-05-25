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

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;
    using Data.Builders;

    /// <summary>
    /// <see cref="Guid"/> 存储标识符生成器。
    /// </summary>
    public class GuidStoreIdentifierGenerator : AbstractStoreIdentifierGenerator<Guid>
    {
        /// <summary>
        /// 构造一个 <see cref="GuidStoreIdentifierGenerator"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public GuidStoreIdentifierGenerator(IOptions<DataBuilderOptions> options,
            IClockService clock, ILoggerFactory loggerFactory)
            : base(options?.Value.IdentifierGenerator, clock, loggerFactory)
        {
        }

    }
}
