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
    /// 抽象存储。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public abstract class AbstractStore<TAccessor> : AbstractStore, IStore<TAccessor>
        where TAccessor : IAccessor
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStore{TAccessor}"/> 实例。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractStore(TAccessor accessor, ILoggerFactory loggerFactory)
            : base(accessor)
        {
            Accessor = accessor;
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractStore{TAccessor}"/> 实例。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractStore(IAccessor accessor, ILoggerFactory loggerFactory)
            : base(accessor, loggerFactory)
        {
            Accessor = accessor.IsValue<IAccessor, TAccessor>(nameof(accessor));
        }


        /// <summary>
        /// 数据访问器。
        /// </summary>
        /// <value>返回 <typeparamref name="TAccessor"/>。</value>
        public new TAccessor Accessor { get; }
    }


    /// <summary>
    /// 抽象存储。
    /// </summary>
    public abstract class AbstractStore : AbstractService, IStore
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStore"/> 实例。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        /// <param name="loggerFactory">给定的 <see cref="ILoggerFactory"/>。</param>
        public AbstractStore(IAccessor accessor, ILoggerFactory loggerFactory)
            : base(loggerFactory)
        {
            Accessor = accessor.NotNull(nameof(accessor));
        }


        /// <summary>
        /// 数据访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        public IAccessor Accessor { get; }
    }
}
