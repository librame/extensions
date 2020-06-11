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
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Librame.Extensions.Data.ValueGenerators
{
    /// <summary>
    /// <see cref="Guid"/> 默认值生成器。
    /// </summary>
    public class GuidDefaultValueGenerator : AbstractValueGenerator<Guid>, IDefaultValueGenerator<Guid>
    {
        /// <summary>
        /// 构造一个 <see cref="GuidDefaultValueGenerator"/>。
        /// </summary>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>（可选）。</param>
        public GuidDefaultValueGenerator(ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
        }


        /// <summary>
        /// 异步获取存储值。
        /// </summary>
        /// <param name="invokeType">给定的调用类型。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <see cref="Guid"/> 的异步操作。</returns>
        public override Task<Guid> GetValueAsync(Type invokeType, CancellationToken cancellationToken = default)
            => cancellationToken.RunFactoryOrCancellationAsync(() =>
            {
                var value = Guid.Empty;
                Logger.LogInformation($"Generate default guid: {value}.");

                return value;
            });
    }
}
