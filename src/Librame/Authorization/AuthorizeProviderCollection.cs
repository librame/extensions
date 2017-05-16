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
    using Providers;
    using Utility;

    /// <summary>
    /// 认证管道集合。
    /// </summary>
    public class AuthorizeProviderCollection : IAuthorizeProviderCollection
    {
        /// <summary>
        /// 构造一个认证管道集合实例。
        /// </summary>
        /// <param name="managers">给定的适配器管理器。</param>
        public AuthorizeProviderCollection(IAuthorizeManagerCollection managers)
        {
            Managers = managers.NotNull(nameof(managers));
        }


        /// <summary>
        /// 认证管理器集合接口。
        /// </summary>
        public virtual IAuthorizeManagerCollection Managers { get; }


        /// <summary>
        /// 获取认证管道。
        /// </summary>
        public virtual IAccountProvider Account
        {
            get
            {
                return SingletonManager.Resolve<IAccountProvider>(key =>
                {
                    return new AccountProviderBase(Managers.Passwd);
                });
            }
        }


        #region SSO

        /// <summary>
        /// 获取应用管道。
        /// </summary>
        public virtual IApplicationProvider Application
        {
            get
            {
                return SingletonManager.Resolve<IApplicationProvider>(key =>
                {
                    return new ApplicationProviderBase(Managers.Ciphertext);
                });
            }
        }

        /// <summary>
        /// 获取令牌管道。
        /// </summary>
        public virtual ITokenProvider Token
        {
            get
            {
                return SingletonManager.Resolve<ITokenProvider>(key =>
                {
                    return new TokenProviderBase(Managers.Ciphertext);
                });
            }
        }

        #endregion


        #region RBAC

        /// <summary>
        /// 获取角色管道。
        /// </summary>
        public virtual IRoleProvider Role
        {
            get
            {
                return SingletonManager.Resolve<IRoleProvider>(key =>
                {
                    return new RoleProviderBase(Managers.Ciphertext);
                });
            }
        }

        /// <summary>
        /// 获取权限管道。
        /// </summary>
        public virtual IPermissionProvider Permission
        {
            get
            {
                return SingletonManager.Resolve<IPermissionProvider>(key =>
                {
                    return new PermissionProviderBase(Managers.Ciphertext);
                });
            }
        }

        #endregion

    }
}
