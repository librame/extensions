#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

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
        /// 构造一个 <see cref="AbstractStore"/> 实例。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public AbstractStore(IAccessor accessor)
            : base(accessor)
        {
            RealAccessor = accessor.IsValue<IAccessor, TAccessor>(nameof(accessor));
        }


        /// <summary>
        /// 真实数据访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        public TAccessor RealAccessor { get; }
    }

    /// <summary>
    /// 抽象存储。
    /// </summary>
    public abstract class AbstractStore : AbstractDisposable, IStore
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStore"/> 实例。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public AbstractStore(IAccessor accessor)
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
