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
            // 认证后重定向回最初请求的 URL 或默认 URL。
            var currentUrl = HttpContext.Current?.Request?.Url.ToString();
            var returnUrl = HttpContext.Current?.Request?.QueryString["returnUrl"];

            if (!string.IsNullOrEmpty(returnUrl) && returnUrl != currentUrl)
            {
                HttpContext.Current?.Response?.Redirect(returnUrl);
            }
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
                    var token = Authorize.Managers.Token.Generate();

                    return new AuthenticateTicket(authInfo.Account, token, DateTime.Now);
                }

                var ticket = removePrincipal?.Invoke(AuthorizeHelper.AUTHORIZATION_KEY);
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
