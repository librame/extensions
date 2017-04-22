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
using System.Web;
using System.Web.Security;

namespace Librame.Authorization.Strategies
{
    using Descriptors;
    using Utility;

    /// <summary>
    /// Web 应用程序认证策略。
    /// </summary>
    public class FormsAuthorizeStrategy : AbstractAuthorizeStrategy, IAuthorizeStrategy
    {
        /// <summary>
        /// 构造一个 <see cref="FormsAuthorizeStrategy"/> 实例。
        /// </summary>
        /// <param name="authSettings">给定的认证首选项。</param>
        /// <param name="provCollection">给定的管道集合。</param>
        public FormsAuthorizeStrategy(AuthorizeSettings authSettings,
            IProviderCollection provCollection)
            : base(authSettings, provCollection)
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
        /// <param name="isPersistent">是否持久化存储。</param>
        /// <param name="bindPrincipal">用于绑定键名与用户的方法。</param>
        protected override void SignInSuccess(AuthenticateInfo authInfo, bool isPersistent,
            Action<string, IPrincipal> bindPrincipal)
        {
            // 构建票根
            var ticket = BuildTicket(authInfo.Account, isPersistent);

            // 构建用户
            var identity = new AccountIdentity(FormsAuthentication.Decrypt(ticket));
            var principal = new AccountPrincipal(identity, null);
            bindPrincipal?.Invoke(AuthorizeHelper.AUTHORIZATION_KEY, principal);

            // 注册票根
            //RegistCookie(ticket);

            // 认证后重定向回最初请求的 URL 或默认 URL。
            //FormsAuthentication.RedirectFromLoginPage(authInfo.Account.Name, isPersistent);
        }

        /// <summary>
        /// 构建认证票根。
        /// </summary>
        /// <param name="account">给定的帐户。</param>
        /// <param name="isPersistent">是否持久化存储。</param>
        /// <returns>返回票根字符串。</returns>
        protected virtual string BuildTicket(IAccountDescriptor account, bool isPersistent)
        {
            // 生成令牌
            var token = AuthorizeHelper.GenerateToken();

            // 生成用户数据
            var descriptor = new TicketDataDescriptor(token, account);
            var userData = TicketDataDescriptor.Serialize(descriptor);
            
            // 创建票根
            var now = DateTime.Now;
            var expiration = now.AddDays(AuthSettings.ExpirationDays);
            var ticket = new FormsAuthenticationTicket(1, account.Name,
                now, expiration, isPersistent, userData,
                FormsAuthentication.FormsCookiePath);

            return FormsAuthentication.Encrypt(ticket);
        }

        ///// <summary>
        ///// 注册票根。
        ///// </summary>
        ///// <param name="ticket">给定的票根。</param>
        //protected virtual void RegistCookie(string ticket)
        //{
        //    // Web 环境
        //    var cookie = new HttpCookie(FormsAuthentication.FormsCookieName, ticket);

        //    // 同步票根与 Cookie 信息
        //    var context = HttpContext.Current;
        //    var decrypt = FormsAuthentication.Decrypt(ticket);

        //    cookie.Expires = decrypt.Expiration;
        //    cookie.Path = decrypt.CookiePath;
        //    cookie.Domain = context?.Request?.Url?.Host;

        //    // 重定向会被清空
        //    context?.Response?.Cookies?.Add(cookie);
        //}


        /// <summary>
        /// 用户登出。
        /// </summary>
        /// <param name="removePrincipal">移除用户方法。</param>
        public override void SignOut(Action<string> removePrincipal)
        {
            try
            {
                if (!AuthSettings.EnableAuthorize)
                    return;

                removePrincipal?.Invoke(AuthorizeHelper.AUTHORIZATION_KEY);

                //FormsAuthentication.SignOut();
            }
            catch (Exception ex)
            {
                Log.Error(ex.AsOrInnerMessage(), ex);
            }
        }

        #endregion

    }
}
