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
    using Utility;

    /// <summary>
    /// 认证管理器集合。
    /// </summary>
    public class AuthorizeManagerCollection : AbstractAdapterCollectionManager,
        IAuthorizeManagerCollection
    {
        /// <summary>
        /// 构造一个认证管理器集合实例。
        /// </summary>
        /// <param name="adapters">给定的适配器管理器。</param>
        public AuthorizeManagerCollection(IAdapterCollection adapters)
            : base(adapters)
        {
        }


        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        public virtual ICiphertextManager Ciphertext
        {
            get
            {
                return SingletonManager.Resolve<ICiphertextManager>(key =>
                {
                    return new CiphertextManager(Adapters);
                });
            }
        }

        /// <summary>
        /// 密码管理器接口。
        /// </summary>
        public virtual IPasswdManager Passwd
        {
            get
            {
                return SingletonManager.Resolve<IPasswdManager>(key =>
                {
                    return new PasswdManager(Adapters);
                });
            }
        }

        /// <summary>
        /// 存储管理器接口。
        /// </summary>
        public virtual IStorageManager Storage
        {
            get
            {
                return SingletonManager.Resolve<IStorageManager>(key =>
                {
                    return new StorageManager(Adapters);
                });
            }
        }

        /// <summary>
        /// 令牌管理器接口。
        /// </summary>
        public virtual ITokenManager Token
        {
            get
            {
                return SingletonManager.Resolve<ITokenManager>(key =>
                {
                    return new TokenManager(Adapters);
                });
            }
        }

    }
}
