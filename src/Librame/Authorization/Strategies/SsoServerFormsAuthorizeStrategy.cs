#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Web;

namespace Librame.Authorization.Strategies
{
    /// <summary>
    /// SSO 窗体服务端应用程序认证策略。
    /// </summary>
    public class SsoServerFormsAuthorizeStrategy : FormsAuthorizeStrategy, IAuthorizeStrategy
    {
        /// <summary>
        /// 构造一个 SSO 窗体服务端应用程序认证策略实例。
        /// </summary>
        /// <param name="authorize">给定的认证适配器接口。</param>
        public SsoServerFormsAuthorizeStrategy(IAuthorizeAdapter authorize)
            : base(authorize)
        {
        }


        #region Sign

        /// <summary>
        /// 登录成功。
        /// </summary>
        /// <param name="authInfo">给定的认证信息。</param>
        /// <param name="principal">给定的帐户用户。</param>
        protected override void SignInSuccess(AuthenticateInfo authInfo, AccountPrincipal principal)
        {
            var ticket = (principal.Identity as AccountIdentity).Ticket;

            // 注册票根
            var encryptTicket = RegistCookie(ticket);

            // 客户端应答链接
            var respondUrl = AuthorizeHelper.FormatRespondUrl(encryptTicket,
                Authorize.AuthSettings.SsoSignInRespondUrl);

            // 重定向到客户端
            HttpContext.Current?.Response?.Redirect(respondUrl);
        }

        #endregion

    }
}
