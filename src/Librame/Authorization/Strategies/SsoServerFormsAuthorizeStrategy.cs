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


        #region Authentication

        /// <summary>
        /// 认证失败。
        /// </summary>
        protected override void OnAuthenticationFailed()
        {
            // 同父级认证策略（重定向到登录页面）
            base.OnAuthenticationFailed();
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
            var ticket = (principal.Identity as AccountIdentity).Ticket;

            // 检查是否为客户端请求
            var pair = AuthorizeHelper.ResolveEncryptAuthIdAndRespondUrl(HttpContext.Current?.Request);
            if (!string.IsNullOrEmpty(pair.Item1) && !string.IsNullOrEmpty(pair.Item2))
            {
                // 编码令牌用于传输
                var encryptToken = Authorize.Managers.Ciphertext.Encode(ticket.Token);

                // 客户端登入应答链接
                var respondUrl = AuthorizeHelper.FormatRespondUrl(encryptToken, pair.Item2);

                // 重定向到客户端
                HttpContext.Current?.Response?.Redirect(respondUrl);
            }
        }

        #endregion

    }
}
