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
    /// 抽象适配器管理器引用。
    /// </summary>
    public abstract class AbstractAdapterManagerReference : LibrameBase<AbstractAdapterManagerReference>,
        IAdapterManagerReference
    {
        /// <summary>
        /// 获取适配器管理器。
        /// </summary>
        public IAdapterManager Adapters { get; }

        /// <summary>
        /// 构造一个 <see cref="AbstractAdapterManagerReference"/> 实例。
        /// </summary>
        /// <param name="adapters">给定的适配器管理器。</param>
        public AbstractAdapterManagerReference(IAdapterManager adapters)
        {
            Adapters = adapters.NotNull(nameof(adapters));
        }

    }
}
