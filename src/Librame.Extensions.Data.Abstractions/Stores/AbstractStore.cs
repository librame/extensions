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

namespace Librame.Extensions.Data.Stores
{
    using Data.Accessors;

    /// <summary>
    /// 抽象存储。
    /// </summary>
    public abstract class AbstractStore : IStore
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStore"/>。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        protected AbstractStore(IAccessor accessor)
        {
            Accessor = accessor.NotNull(nameof(accessor));
        }


        /// <summary>
        /// 访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        public IAccessor Accessor { get; }

        /// <summary>
        /// 日志工厂。
        /// </summary>
        public ILoggerFactory LoggerFactory
            => Accessor.LoggerFactory;
    }
}
