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
    /// 抽象存储标识生成器。
    /// </summary>
    public abstract class AbstractStoreIdentityGenerator : AbstractService, IStoreIdentityGenerator
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreIdentityGenerator"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="factory">给定的 <see cref="IIdentificationGeneratorFactory"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractStoreIdentityGenerator(IClockService clock,
            IIdentificationGeneratorFactory factory, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Clock = clock.NotNull(nameof(clock));
            Factory = factory.NotNull(nameof(factory));
        }


        /// <summary>
        /// 时钟服务。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock { get; }

        /// <summary>
        /// 标识生成器工厂。
        /// </summary>
        /// <value>返回 <see cref="IIdentificationGeneratorFactory"/>。</value>
        public IIdentificationGeneratorFactory Factory { get; }


        /// <summary>
        /// 生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <returns>返回 <typeparamref name="TId"/>。</returns>
        public virtual TId GenerateId<TId>(string idName)
            where TId : IEquatable<TId>
        {
            var generator = Factory.GetIdGenerator<TId>();

            var id = generator.GenerateId(Clock);
            Logger.LogTrace($"Generate {generator.IdType.Name} {idName}: {id}.");

            return id;
        }

        /// <summary>
        /// 异步生成标识。
        /// </summary>
        /// <param name="idName">给定的标识名称。</param>
        /// <param name="cancellationToken">给定的 <see cref="CancellationToken"/>（可选）。</param>
        /// <returns>返回一个包含 <typeparamref name="TId"/> 的异步操作。</returns>
        public virtual async Task<TId> GenerateIdAsync<TId>(string idName,
            CancellationToken cancellationToken = default)
            where TId : IEquatable<TId>
        {
            var generator = Factory.GetIdGenerator<TId>();

            var id = await generator.GenerateIdAsync(Clock, cancellationToken).ConfigureAwait();
            Logger.LogTrace($"Generate {generator.IdType.Name} {idName}: {id}.");

            return id;
        }

    }
}
