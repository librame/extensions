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
    /// SSO Web 客户端应用程序认证策略。
    /// </summary>
    public class SsoClientFormsAuthorizeStrategy : FormsAuthorizeStrategy, IAuthorizeStrategy
    {
        /// <summary>
        /// 构造一个 <see cref="SsoClientFormsAuthorizeStrategy"/> 实例。
        /// </summary>
        /// <param name="authSettings">给定的认证首选项。</param>
        /// <param name="provCollection">给定的管道集合。</param>
        public SsoClientFormsAuthorizeStrategy(AuthorizeSettings authSettings,
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
            // 使用框架授权编号
            var authServerUrl = AuthorizeHelper.FormatSsoServerSignInUrl(AuthSettings.SsoServerSignInUrl,
                AuthSettings.AdapterSettings.AuthId,
                AuthSettings.SsoSignInRespondUrl);
            
            // 客户端模式则需重定向到认证服务器
            HttpContext.Current?.Response.Redirect(authServerUrl);
        }

        #endregion


        #region Sign

        /// <summary>
        /// 用户登入。
        /// </summary>
        /// <param name="name">给定的用户名（此参数无效）。</param>
        /// <param name="passwd">给定的密码（此参数无效）。</param>
        /// <param name="isPersistent">是否持久化存储（此参数无效）。</param>
        /// <param name="bindPrincipal">用于绑定键名与用户的方法（此参数有效）。</param>
        /// <returns>返回认证信息。</returns>
        public override AuthenticateInfo SignIn(string name, string passwd, bool isPersistent,
            Action<string, IPrincipal> bindPrincipal)
        {
            bindPrincipal.GuardNull(nameof(bindPrincipal));

            try
            {
                IAccountDescriptor account = null;

                if (!AuthSettings.EnableAuthorize)
                {
                    // 禁用认证，默认模拟已认证用户
                    account = new AccountDescriptor("VirtualUser", string.Empty, AccountStatus.Active);
                    return new AuthenticateInfo(account, "当前已禁用认证，默认使用模拟认证用户");
                }

                // 解析认证服务器应答链接参数
                var request = HttpContext.Current?.Request;
                var ticket = AuthorizeHelper.ResolveSsoSignInRespondUrl(request);

                // 构建用户
                var identity = new AccountIdentity(FormsAuthentication.Decrypt(ticket));
                var principal = new AccountPrincipal(identity, null);
                bindPrincipal?.Invoke(AuthorizeHelper.AUTHORIZATION_KEY, principal);

                // 模拟帐户实例
                account = new AccountDescriptor(identity.Name, string.Empty, AccountStatus.Active);
                return new AuthenticateInfo(account, "服务端认证成功");
            }
            catch (Exception ex)
            {
                Log.Error(ex.AsOrInnerMessage(), ex);

                return null;
            }
        }

        #endregion

    }
}
