#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

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
        /// 构造一个 <see cref="AbstractStore{TAccessor}"/>。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public AbstractStore(IAccessor accessor)
            : base(accessor)
        {
            // Cast
            Accessor = accessor.CastTo<IAccessor, TAccessor>(nameof(accessor));
        }

        /// <summary>
        /// 构造一个 <see cref="AbstractStore{TAccessor}"/>。
        /// </summary>
        /// <param name="accessor">给定的 <typeparamref name="TAccessor"/>。</param>
        public AbstractStore(TAccessor accessor)
            : base(accessor)
        {
            // Override
            Accessor = accessor;
        }


        /// <summary>
        /// 覆盖数据访问器接口实例。
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
        /// 构造一个 <see cref="AbstractStore"/>。
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
        /// 服务提供程序。
        /// </summary>
        public IServiceProvider ServiceProvider
            => Accessor.ServiceProvider;

        /// <summary>
        /// 服务工厂。
        /// </summary>
        public ServiceFactoryDelegate ServiceFactory
            => Accessor.ServiceFactory;


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
