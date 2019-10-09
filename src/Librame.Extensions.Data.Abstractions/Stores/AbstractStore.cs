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
        /// 访问器。
        /// </summary>
        /// <value>返回 <see cref="IAccessor"/>。</value>
        public IAccessor Accessor { get; }


        /// <summary>
        /// 内部服务提供程序。
        /// </summary>
        /// <value>返回 <see cref="IServiceProvider"/>。</value>
        public IServiceProvider InternalServiceProvider
            => Accessor.InternalServiceProvider;

        /// <summary>
        /// 服务工厂。
        /// </summary>
        /// <value>返回 <see cref="ServiceFactoryDelegate"/>。</value>
        public ServiceFactoryDelegate ServiceFactory
            => Accessor.ServiceFactory;


        /// <summary>
        /// 释放存储。
        /// </summary>
        protected override void DisposeCore()
        {
        }
    }
}
