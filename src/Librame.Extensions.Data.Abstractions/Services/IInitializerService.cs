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
    /// 初始化器服务。
    /// </summary>
    /// <typeparam name="TAccessor">指定的访问器类型。</typeparam>
    public interface IInitializerService<in TAccessor> : IService
        where TAccessor : IAccessor
    {
        /// <summary>
        /// 初始化数据。
        /// </summary>
        /// <param name="storeHub">给定的 <see cref="IStoreHub{TAccessor}"/>。</param>
        void Initialize(IStoreHub<TAccessor> storeHub);
    }
}
