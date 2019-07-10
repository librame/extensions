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
        /// 构造一个抽象存储实例（可用于容器构造）。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public AbstractStore(IAccessor accessor)
            : base(accessor)
        {
            // Cast
            Accessor = accessor.CastTo<IAccessor, TAccessor>(nameof(accessor));
        }

        /// <summary>
        /// 构造一个抽象存储实例（可用于手动构造）。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        protected AbstractStore(TAccessor accessor)
            : base(accessor)
        {
            // Override
            Accessor = accessor;
        }


        /// <summary>
        /// 覆盖数据访问器。
        /// </summary>
        /// <value>返回 <typeparamref name="TAccessor"/>。</value>
        public new TAccessor Accessor { get; }
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


        /// <summary>
        /// 处置对象。
        /// </summary>
        public override void Dispose()
        {
            Accessor.Dispose();

            base.Dispose();
        }

    }
}
