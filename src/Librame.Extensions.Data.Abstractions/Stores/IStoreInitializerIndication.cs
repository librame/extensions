#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Data.Stores
{
    using Core;
    using Core.Services;
    using Data.Accessors;

    /// <summary>
    /// 存储初始化器标示接口（用于标记、约束实例）。
    /// </summary>
    public interface IStoreInitializerIndication : IService, IIndication
    {
        /// <summary>
        /// 时钟服务。
        /// </summary>
        /// <value>返回 <see cref="IClockService"/>。</value>
        IClockService Clock { get; }

        /// <summary>
        /// 需要保存变化。
        /// </summary>
        bool RequiredSaveChanges { get; }


        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        bool IsInitialized(IAccessor accessor);
    }
}
