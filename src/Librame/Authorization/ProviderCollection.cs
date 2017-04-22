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
    using Providers;
    using Utility;

    /// <summary>
    /// 管道集合。
    /// </summary>
    public class ProviderCollection : AbstractAdapterManagerReference, IProviderCollection
    {
        /// <summary>
        /// 构造一个 <see cref="ProviderCollection"/> 实例。
        /// </summary>
        /// <param name="adapters">给定的适配器管理器。</param>
        public ProviderCollection(IAdapterManager adapters)
            : base(adapters)
        {
        }


        /// <summary>
        /// 获取认证管道。
        /// </summary>
        public virtual IAccountProvider Account
        {
            get { return SingletonManager.Resolve<IAccountProvider>(key => new AccountProviderBase(Adapters)); }
        }


        #region SSO

        /// <summary>
        /// 获取应用管道。
        /// </summary>
        public virtual IApplicationProvider Application
        {
            get { return SingletonManager.Resolve<IApplicationProvider>(key => new ApplicationProviderBase(Adapters)); }
        }

        /// <summary>
        /// 获取令牌管道。
        /// </summary>
        public virtual ITokenProvider Token
        {
            get { return SingletonManager.Resolve<ITokenProvider>(key => new TokenProviderBase(Adapters)); }
        }

        #endregion


        #region RBAC

        /// <summary>
        /// 获取角色管道。
        /// </summary>
        public virtual IRoleProvider Role
        {
            get { return SingletonManager.Resolve<IRoleProvider>(key => new RoleProviderBase(Adapters)); }
        }

        /// <summary>
        /// 获取权限管道。
        /// </summary>
        public virtual IPermissionProvider Permission
        {
            get { return SingletonManager.Resolve<IPermissionProvider>(key => new PermissionProviderBase(Adapters)); }
        }

        #endregion

    }
}
