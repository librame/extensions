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
        /// 获取认证适配器接口。
        /// </summary>
        IAuthorizeAdapter Authorize { get; }


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
        /// <returns>返回用户票根。</returns>
        AuthenticateTicket SignOut(Func<string, AuthenticateTicket> removePrincipal);

        #endregion


        //#region Ticket

        ///// <summary>
        ///// 加密票根。
        ///// </summary>
        ///// <param name="ticket">给定的认证票根。</param>
        ///// <returns>返回票根字符串。</returns>
        //string EncryptTicket(AuthenticateTicket ticket);

        ///// <summary>
        ///// 解密票根。
        ///// </summary>
        ///// <param name="ticket">给定的票根字符串。</param>
        ///// <returns>返回认证票根。</returns>
        //AuthenticateTicket DecryptTicket(string ticket);

        //#endregion

    }
}
