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
    /// <summary>
    /// <see cref="Guid"/> 数据存储初始化器。
    /// </summary>
    /// <typeparam name="TIncremId">指定的增量式标识类型。</typeparam>
    public class GuidDataStoreInitializer<TIncremId> : AbstractDataStoreInitializer<Guid, TIncremId, Guid>
        where TIncremId : IEquatable<TIncremId>
    {
        /// <summary>
        /// 构造一个 <see cref="GuidDataStoreInitializer{TIncremId}"/>。
        /// </summary>
        /// <param name="identifierGenerator">给定的 <see cref="IStoreIdentifierGenerator"/>。</param>
        /// <param name="validator">给定的 <see cref="IStoreInitializationValidator"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public GuidDataStoreInitializer(IStoreIdentifierGenerator identifierGenerator,
            IStoreInitializationValidator validator, ILoggerFactory loggerFactory)
            : base(identifierGenerator, validator, loggerFactory)
        {
        }

    }
}
