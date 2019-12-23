#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Stores
{
    using Accessors;

    /// <summary>
    /// 抽象存储中心。
    /// </summary>
    public abstract class AbstractStoreHub : AbstractStore, IStoreHub
    {
        /// <summary>
        /// 构造一个 <see cref="AbstractStoreHub"/>。
        /// </summary>
        /// <param name="initializer">给定的 <see cref="IStoreInitializer"/>。</param>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        public AbstractStoreHub(IStoreInitializer initializer, IAccessor accessor)
            : base(accessor)
        {
            Initializer = initializer.NotNull(nameof(initializer));
        }


        /// <summary>
        /// 存储初始化器。
        /// </summary>
        /// <value>返回 <see cref="IStoreInitializer"/>。</value>
        public IStoreInitializer Initializer { get; }
    }
}
