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

namespace Librame.Authorization.Managers
{
    using Adaptation;

    /// <summary>
    /// 令牌管理器接口。
    /// </summary>
    public interface ITokenManager : IAdapterCollectionManager
    {
        /// <summary>
        /// 生成令牌。
        /// </summary>
        /// <returns>返回令牌字符串。</returns>
        string Generate();

        /// <summary>
        /// 生成令牌。
        /// </summary>
        /// <param name="guid">给定的 GUID。</param>
        /// <returns>返回令牌字符串。</returns>
        string Generate(Guid guid);
    }
}
