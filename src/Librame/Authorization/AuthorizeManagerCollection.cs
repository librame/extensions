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
    public class AuthorizeManagerCollection : AbstractAdapterManagerReference,
        IAuthorizeManagerCollection
    {
        /// <summary>
        /// 构造一个认证管理器集合实例。
        /// </summary>
        /// <param name="adapters">给定的适配器管理器。</param>
        public AuthorizeManagerCollection(IAdapterManager adapters)
            : base(adapters)
        {
        }


        /// <summary>
        /// 密文管理器接口。
        /// </summary>
        public virtual ICryptogramManager Cryptogram
        {
            get
            {
                return SingletonManager.Resolve<ICryptogramManager>(key =>
                {
                    return new CryptogramManager(Adapters);
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

    }
}
