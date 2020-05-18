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

namespace Librame.Extensions.Data.Aspects
{
    using Core.Services;
    using Data.Builders;
    using Data.Stores;

    /// <summary>
    /// 数据库上下文访问器截面依赖集合。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public class DbContextAccessorAspectDependencies<TGenId> : AbstractExtensionBuilderService<DataBuilderOptions>
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 构造一个 <see cref="DbContextAccessorAspectDependencies{TGenId}"/>。
        /// </summary>
        /// <param name="clock">给定的 <see cref="IClockService"/>。</param>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator{TGenId}"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{DataBuilderOptions}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public DbContextAccessorAspectDependencies(IClockService clock,
            IStoreIdentifierGenerator<TGenId> identifierGenerator,
            IOptions<DataBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(options, loggerFactory)
        {
            Clock = clock.NotNull(nameof(clock));
            IdentifierGenerator = identifierGenerator.NotNull(nameof(identifierGenerator));
        }


        /// <summary>
        /// 时钟服务。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock { get; }

        /// <summary>
        /// 存储标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IStoreIdentifierGenerator{TGenId}"/>。</value>
        public IStoreIdentifierGenerator<TGenId> IdentifierGenerator { get; }
    }
}
