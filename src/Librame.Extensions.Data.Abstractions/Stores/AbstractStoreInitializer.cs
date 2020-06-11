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

namespace Librame.Extensions.Data.Stores
{
    using Core.Services;
    using Data.Accessors;

    /// <summary>
    /// 抽象存储初始化器。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public abstract class AbstractStoreInitializer<TGenId> : AbstractService, IStoreInitializer<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreInitializer{TGenId}"/>。
        /// </summary>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator{TGenId}"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractStoreInitializer(IStoreIdentifierGenerator<TGenId> identifierGenerator,
            ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            IdentifierGenerator = identifierGenerator.NotNull(nameof(identifierGenerator));
        }


        /// <summary>
        /// 存储标识符生成器。
        /// </summary>
        /// <value>返回 <see cref="IStoreIdentifierGenerator{TGenId}"/>。</value>
        public IStoreIdentifierGenerator<TGenId> IdentifierGenerator { get; }

        /// <summary>
        /// 时钟服务。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        public IClockService Clock
            => IdentifierGenerator.Clock;


        /// <summary>
        /// 需要保存变化。
        /// </summary>
        public bool RequiredSaveChanges { get; protected set; }


        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public abstract bool IsInitialized(IAccessor accessor);

        /// <summary>
        /// 初始化存储。
        /// </summary>
        /// <param name="stores">给定的 <see cref="IStoreHub{TGenId}"/>。</param>
        public abstract void Initialize(IStoreHub<TGenId> stores);
    }
}
