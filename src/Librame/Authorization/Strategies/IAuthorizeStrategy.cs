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
using System.Security.Principal;

namespace Librame.Authorization.Strategies
{
    /// <summary>
    /// 认证策略接口。
    /// </summary>
    public interface IAuthorizeStrategy
    {
        /// <summary>
        /// 获取或设置认证首选项。
        /// </summary>
        AuthorizeSettings AuthSettings { get; }

        /// <summary>
        /// 获取管道工厂。
        /// </summary>
        IProviderCollection ProvCollection { get; }


        #region Authentication

        /// <summary>
        /// 是否已认证。
        /// </summary>
        /// <param name="principalFactory">给定取得用户的方法。</param>
        /// <returns>返回是否已认证的布尔值。</returns>
        bool IsAuthenticated(Func<string, IPrincipal> principalFactory);

        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="principalFactory">给定取得用户的方法。</param>
        void OnAuthentication(Func<string, IPrincipal> principalFactory);

        #endregion


        #region Sign

        /// <summary>
        /// 用户登入。
        /// </summary>
        /// <param name="name">给定的用户名。</param>
        /// <param name="passwd">给定的密码。</param>
        /// <param name="isPersistent">是否持久化存储。</param>
        /// <param name="bindPrincipal">用于绑定键名与用户的方法。</param>
        /// <returns>返回认证信息。</returns>
        AuthenticateInfo SignIn(string name, string passwd, bool isPersistent,
            Action<string, IPrincipal> bindPrincipal);

        /// <summary>
        /// 用户登出。
        /// </summary>
        /// <param name="removePrincipal">移除用户方法。</param>
        void SignOut(Action<string> removePrincipal);

        #endregion

    }
}
