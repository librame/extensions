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

namespace Librame.Extensions.Core.Services
{
    using Threads;

    /// <summary>
    /// 抽象并发服务。
    /// </summary>
    public abstract class AbstractConcurrentService : AbstractService, IConcurrentService
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractService"/>。
        /// </summary>
        /// <param name="locker">给定的 <see cref="IMemoryLocker"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        protected AbstractConcurrentService(IMemoryLocker locker, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Locker = locker.NotNull(nameof(locker));
        }


        /// <summary>
        /// 内存锁定器。
        /// </summary>
        public IMemoryLocker Locker { get; }
    }
}
