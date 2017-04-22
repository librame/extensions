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
    /// <summary>
    /// SSO Web 服务端应用程序认证策略。
    /// </summary>
    public class SsoServerFormsAuthorizeStrategy : FormsAuthorizeStrategy, IAuthorizeStrategy
    {
        /// <summary>
        /// 构造一个 <see cref="SsoServerFormsAuthorizeStrategy"/> 实例。
        /// </summary>
        /// <param name="authSettings">给定的认证首选项。</param>
        /// <param name="provCollection">给定的管道集合。</param>
        public SsoServerFormsAuthorizeStrategy(AuthorizeSettings authSettings,
            IProviderCollection provCollection)
            : base(authSettings, provCollection)
        {
        }


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
            
            // 客户端应答链接
            var respondUrl = AuthorizeHelper.FormatSsoSignInRespondUrl(AuthSettings.SsoSignInRespondUrl, ticket);

            // 重定向到客户端
            HttpContext.Current?.Response?.Redirect(respondUrl);
        }

        #endregion

    }
}
