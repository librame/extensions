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


        /// <summary>
        /// 获取服务。
        /// </summary>
        /// <typeparam name="TService">指定的服务类型。</typeparam>
        /// <param name="isRequired">是必需的服务（可选；默认必需，不存在将抛出异常）。</param>
        /// <returns>返回 <typeparamref name="TService"/>。</returns>
        public virtual TService GetService<TService>(bool isRequired = true)
            => Accessor.GetService<TService>(isRequired);

        /// <summary>
        /// 获取服务提供程序。
        /// </summary>
        /// <returns>返回 <see cref="IServiceProvider"/>。</returns>
        public virtual IServiceProvider GetServiceProvider()
            => Accessor.GetServiceProvider();
    }
}
