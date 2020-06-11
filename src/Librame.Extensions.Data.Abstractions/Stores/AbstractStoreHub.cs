#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Data.Stores
{
    using Accessors;

    /// <summary>
    /// 抽象存储中心。
    /// </summary>
    /// <typeparam name="TGenId">指定的生成式标识类型。</typeparam>
    public abstract class AbstractStoreHub<TGenId> : AbstractStore, IStoreHub<TGenId>
        where TGenId : IEquatable<TGenId>
    {
        /// <summary>
        /// 构造一个存储中心。
        /// </summary>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer{TGenId}"/>。</param>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        protected AbstractStoreHub(IStoreInitializer<TGenId> initializer, IAccessor accessor)
            : base(accessor)
        {
            Initializer = initializer.NotNull(nameof(initializer));
        }


        /// <summary>
        /// 存储初始化器。
        /// </summary>
        /// <value>返回 <see cref="IStoreInitializer{TGenId}"/>。</value>
        public IStoreInitializer<TGenId> Initializer { get; }
    }
}
