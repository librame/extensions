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

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;
    using Data.Builders;

    /// <summary>
    /// <see cref="Guid"/> 存储标识符。
    /// </summary>
    public class GuidStoreIdentifier : AbstractStoreIdentifier<Guid>
    {
        private readonly DataBuilderOptions _options;


        /// <summary>
        /// 构造一个 <see cref="GuidStoreIdentifier"/>。
        /// </summary>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public GuidStoreIdentifier(IOptions<DataBuilderOptions> options,
            IClockService clock, ILoggerFactory loggerFactory)
            : base(clock, loggerFactory)
        {
            _options = options.NotNull(nameof(options)).Value;
        }


        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// DataBuilderOptions.SUIDGenerator is null.
        /// </exception>
        /// <param name="idTraceName">标识跟踪名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="Guid"/> 的异步操作。</returns>
        protected override async Task<Guid> GenerateIdAsync(string idTraceName,
            CancellationToken cancellationToken = default)
        {
            _options.SUIDGenerator.NotNull(nameof(_options.SUIDGenerator));

            var guid = await _options.SUIDGenerator.GenerateAsync(Clock, isUtc: true, cancellationToken)
                .ConfigureAndResultAsync();
            Logger.LogTrace($"Generate {idTraceName}: {guid}");

            return guid;
        }

    }
}
