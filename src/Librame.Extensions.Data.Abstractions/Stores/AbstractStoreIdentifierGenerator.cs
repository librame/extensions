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

namespace Librame.Extensions.Data.Stores
{
    using Core.Identifiers;
    using Core.Services;

    /// <summary>
    /// 抽象存储标识符生成器。
    /// </summary>
    /// <typeparam name="TId">指定的标识类型。</typeparam>
    public abstract class AbstractStoreIdentifierGenerator<TId> : AbstractService, IStoreIdentifierGenerator
        where TId : IEquatable<TId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreIdentifierGenerator{TId}"/>。
        /// </summary>
        /// <param name="generator">给定的 <see cref="IIdentifierGenerator{TId}"/>。</param>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractStoreIdentifierGenerator(IIdentifierGenerator<TId> generator, IClockService clock,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Generator = generator.NotNull(nameof(generator));
            Clock = clock.NotNull(nameof(clock));
        }


        /// <summary>
        /// 标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IIdentifierGenerator{TId}"/>。</value>
        public IIdentifierGenerator<TId> Generator { get; }

        /// <summary>
        /// 时钟。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock { get; }


        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public abstract Task<TId> GenerateIdAsync(string idName, CancellationToken cancellationToken = default);
    }
}
