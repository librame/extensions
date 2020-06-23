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
    using Core.Services;
    using Data.Accessors;

    /// <summary>
    /// 存储初始化验证器接口。
    /// </summary>
    public interface IStoreInitializationValidator : IService
    {
        /// <summary>
        /// 是否已完成初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        bool IsInitialized(IAccessor accessor);

        /// <summary>
        /// 设置已完成初始化。
        /// </summary>
        /// <param name="accessor">给定的 <see cref="IAccessor"/>。</param>
        void SetInitialized(IAccessor accessor);
    }
}
