#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Authorization
{
    using Adaptation;
    using Managers;

    /// <summary>
    /// 认证管理器集合接口。
    /// </summary>
    public interface IAuthorizeManagerCollection : IAdapterCollectionManager
    {
        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        ICiphertextManager Ciphertext { get; }

        /// <summary>
        /// 密码管理器接口。
        /// </summary>
        IPasswdManager Passwd { get; }

        /// <summary>
        /// 存储管理器接口。
        /// </summary>
        IStorageManager Storage { get; }

        /// <summary>
        /// 令牌管理器接口。
        /// </summary>
        ITokenManager Token { get; }
    }
}
