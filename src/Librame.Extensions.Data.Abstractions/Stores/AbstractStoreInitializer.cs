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

namespace Librame.Extensions.Data
{
    using Core;

    /// <summary>
    /// 抽象存储初始化器。
    /// </summary>
    public abstract class AbstractStoreInitializer : IStoreInitializer
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreInitializer"/>。
        /// </summary>
        /// <param name="identifier">给定的 <see cref="IStoreIdentifier"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractStoreInitializer(IStoreIdentifier identifier, ILoggerFactory loggerFactory)
        {
            Identifier = identifier.NotNull(nameof(identifier));
            LoggerFactory = loggerFactory.NotNull(nameof(loggerFactory));
        }


        /// <summary>
        /// 时钟服务。
        /// </summary>
        public IClockService Clock
            => Identifier.Clock;

        /// <summary>
        /// 标识符。
        /// </summary>
        public IStoreIdentifier Identifier { get; }

        /// <summary>
        /// 日志工厂。
        /// </summary>
        public ILoggerFactory LoggerFactory { get; }


        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        public bool IsInitialized { get; protected set; }

        /// <summary>
        /// 需要保存变化。
        /// </summary>
        public bool RequiredSaveChanges { get; protected set; }


        /// <summary>
        /// 日志。
        /// </summary>
        protected ILogger Logger
            => LoggerFactory.CreateLogger(GetType());
    }
}
