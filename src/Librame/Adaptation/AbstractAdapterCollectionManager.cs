#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Adaptation
{
    using Utility;
    
    /// <summary>
    /// 抽象适配器集合管理器。
    /// </summary>
    public abstract class AbstractAdapterCollectionManager : LibrameBase<AbstractAdapterCollectionManager>,
        IAdapterCollectionManager
    {
        /// <summary>
        /// 获取适配器管理器。
        /// </summary>
        public IAdapterCollection Adapters { get; }

        /// <summary>
        /// 构造一个 <see cref="AbstractAdapterCollectionManager"/> 实例。
        /// </summary>
        /// <param name="adapters">给定的适配器管理器。</param>
        public AbstractAdapterCollectionManager(IAdapterCollection adapters)
        {
            Adapters = adapters.NotNull(nameof(adapters));
        }

    }
}
