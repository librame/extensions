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

    /// <summary>
    /// 管道集合接口。
    /// </summary>
    public interface IProviderCollection : IAdapterManagerReference
    {
        /// <summary>
        /// 获取认证管道。
        /// </summary>
        IAccountProvider Account { get; }


        #region SSO

        /// <summary>
        /// 获取应用管道。
        /// </summary>
        IApplicationProvider Application { get; }

        /// <summary>
        /// 获取令牌管道。
        /// </summary>
        ITokenProvider Token { get; }

        #endregion


        #region RBAC

        /// <summary>
        /// 获取角色管道。
        /// </summary>
        IRoleProvider Role { get; }

        /// <summary>
        /// 获取权限管道。
        /// </summary>
        IPermissionProvider Permission { get; }

        #endregion

    }
}
