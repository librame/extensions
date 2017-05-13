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
using System.Web;
using System.Web.Security;

namespace Librame.Authorization.Strategies
{
    using Utility;

    /// <summary>
    /// 窗体应用程序认证策略。
    /// </summary>
    public class FormsAuthorizeStrategy : AbstractAuthorizeStrategy, IAuthorizeStrategy
    {
        /// <summary>
        /// 构造一个窗体应用程序认证策略实例。
        /// </summary>
        /// <param name="authorize">给定的认证适配器接口。</param>
        public FormsAuthorizeStrategy(IAuthorizeAdapter authorize)
            : base(authorize)
        {
        }


        #region Authentication

        /// <summary>
        /// 认证失败。
        /// </summary>
        protected override void OnAuthenticationFailed()
        {
            // 重定向到登录页面
            FormsAuthentication.RedirectToLoginPage();
        }

        #endregion


        #region Sign

        /// <summary>
        /// 登录成功。
        /// </summary>
        /// <param name="authInfo">给定的认证信息。</param>
        /// <param name="principal">给定的帐户用户。</param>
        protected override void SignInSuccess(AuthenticateInfo authInfo, AccountPrincipal principal)
        {
            // 注册票根
            RegistCookie((principal.Identity as AccountIdentity).Ticket);

            // 认证后重定向回最初请求的 URL 或默认 URL。
            var currentUrl = HttpContext.Current?.Request?.Url.ToString();
            var returnUrl = HttpContext.Current?.Request?.QueryString["returnUrl"];
            if (!string.IsNullOrEmpty(returnUrl) && returnUrl != currentUrl)
            {
                HttpContext.Current?.Response?.Redirect(returnUrl);
            }
        }

        ///// <summary>
        ///// 构建认证票根。
        ///// </summary>
        ///// <param name="account">给定的帐户。</param>
        ///// <param name="isPersistent">是否持久化存储。</param>
        ///// <returns>返回票根字符串。</returns>
        //protected virtual string BuildTicket(IAccountDescriptor account, bool isPersistent)
        //{
        //    var ticket = new AuthenticateTicket(account, DateTime.Now, null, isPersistent);

        //    return EncryptTicket(ticket);

        //    //// 生成令牌
        //    //var token = AuthorizeHelper.GenerateToken();

        //    //// 生成用户数据
        //    //var descriptor = new TicketDataDescriptor(token, account);
        //    //var userData = TicketDataDescriptor.Serialize(descriptor);

        //    //// 创建票根
        //    //var now = DateTime.Now;
        //    //var expiration = now.AddDays(AuthSettings.ExpirationDays);
        //    //var ticket = new FormsAuthenticationTicket(1, account.Name,
        //    //    now, expiration, isPersistent, userData,
        //    //    FormsAuthentication.FormsCookiePath);

        //    //return FormsAuthentication.Encrypt(ticket);
        //}

        /// <summary>
        /// 注册票根。
        /// </summary>
        /// <param name="ticket">给定的票根。</param>
        /// <returns>返回加密后的票根字符串。</returns>
        protected virtual string RegistCookie(AuthenticateTicket ticket)
        {
            // Web 环境
            var encrypt = EncryptTicket(ticket);
            var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, encrypt);

            // 同步票根与 Cookie 信息
            var context = HttpContext.Current;

            cookie.Expires = ticket.Expiration;
            cookie.Path = ticket.Path;
            cookie.Domain = context?.Request?.Url?.Host;

            // 重定向会被清空
            context?.Response?.Cookies?.Add(cookie);

            return encrypt;
        }

        /// <summary>
        /// 移除票根。
        /// </summary>
        /// <returns>返回票根字符串。</returns>
        protected virtual string RemoveCookie()
        {
            var cookies = HttpContext.Current?.Response?.Cookies;

            if (!ReferenceEquals(cookies, null))
            {
                var ticket = cookies[FormsAuthentication.FormsCookieName].Value;
                cookies.Remove(FormsAuthentication.FormsCookieName);

                return ticket;
            }

            return string.Empty;
        }


        /// <summary>
        /// 用户登出。
        /// </summary>
        /// <param name="removePrincipal">移除用户方法。</param>
        /// <returns>返回用户票根。</returns>
        public override AuthenticateTicket SignOut(Func<string, AuthenticateTicket> removePrincipal)
        {
            try
            {
                if (!Authorize.AuthSettings.EnableAuthorize)
                {
                    var authInfo = GetSimulateAuthenticateInfo();
                    return new AuthenticateTicket(authInfo.Account, DateTime.Now);
                }

                var ticket = removePrincipal?.Invoke(AuthorizeHelper.AUTHORIZATION_KEY);

                RemoveCookie();

                return ticket;
            }
            catch (Exception ex)
            {
                Log.Error(ex.InnerMessage(), ex);

                throw ex;
            }
        }

        #endregion

    }
}
