#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Librame;
using Librame.Authorization;
using Librame.Authorization.Strategies;
using System.Web.Security;
using System.Web.SessionState;

namespace System.Web
{
    /// <summary>
    /// <see cref="IAuthorizeStrategy"/> 静态扩展。
    /// </summary>
    public static class AuthorizeStrategyExtensions
    {
        /// <summary>
        /// 解析登入链接（支持常规、SSO 客/服端模式）。
        /// </summary>
        /// <param name="session">给定的会话。</param>
        /// <returns>返回登入链接字符串。</returns>
        public static string ResolveLoginUrl(this HttpSessionStateBase session)
        {
            var authorize = LibrameArchitecture.Adapters.Authorization;

            // 如果未启用 SSO 或启用且是服务端模式
            if (!authorize.AuthSettings.EnableSso || authorize.AuthSettings.IsSsoServerMode)
                return FormsAuthentication.LoginUrl;

            var encryptAuthId = authorize.Managers.Ciphertext
                .Encode(authorize.AuthSettings.AdapterSettings.AuthId);

            return AuthorizeHelper.FormatServerSignInUrl(encryptAuthId,
                authorize.AuthSettings.SsoSignInRespondUrl,
                authorize.AuthSettings.SsoServerSignInUrl);
        }

        /// <summary>
        /// 解析登出链接。
        /// </summary>
        /// <param name="session">给定的会话。</param>
        /// <returns>返回登出链接字符串。</returns>
        public static string ResolveLogoutUrl(this HttpSessionStateBase session)
        {
            var authorize = LibrameArchitecture.Adapters.Authorization;

            // 如果未启用 SSO 或启用且是服务端模式
            if (!authorize.AuthSettings.EnableSso || authorize.AuthSettings.IsSsoServerMode)
                return "#";

            var ticket = session.ResolveTicket();
            if (ticket == null)
                return "#";

            var encryptToken = authorize.Managers.Ciphertext.Encode(ticket.Token);
            var encryptAuthId = authorize.Managers.Ciphertext.Encode(authorize.Settings.AuthId);

            var signOutUrl = AuthorizeHelper.FormatServerSignOutUrl(encryptToken, encryptAuthId,
                authorize.AuthSettings.SsoSignOutRespondUrl,
                authorize.AuthSettings.SsoServerSignOutUrl);

            return signOutUrl;
        }


        /// <summary>
        /// 解析用户信息。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <returns>返回 <see cref="AccountPrincipal"/>。</returns>
        public static AccountPrincipal ResolvePrincipal(this HttpSessionState session)
        {
            return (AccountPrincipal)session?[AuthorizeHelper.AUTHORIZATION_KEY];
        }
        /// <summary>
        /// 解析用户信息。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <returns>返回 <see cref="AccountPrincipal"/>。</returns>
        public static AccountPrincipal ResolvePrincipal(this HttpSessionStateBase session)
        {
            return (AccountPrincipal)session?[AuthorizeHelper.AUTHORIZATION_KEY];
        }
        

        /// <summary>
        /// 解析用户票根。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <returns>返回认证票根。</returns>
        public static AuthenticateTicket ResolveTicket(this HttpSessionState session)
        {
            return session.ResolvePrincipal()?.AsTicket();
        }
        /// <summary>
        /// 解析用户票根。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <returns>返回认证票根。</returns>
        public static AuthenticateTicket ResolveTicket(this HttpSessionStateBase session)
        {
            return session.ResolvePrincipal()?.AsTicket();
        }


        /// <summary>
        /// 是否已认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAuthenticated(this HttpSessionState session)
        {
            var strategy = LibrameArchitecture.Adapters.Authorization.Strategy;

            return strategy.IsAuthenticated(k => (AccountPrincipal)session?[k]);
        }
        /// <summary>
        /// 是否已认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAuthenticated(this HttpSessionStateBase session)
        {
            var strategy = LibrameArchitecture.Adapters.Authorization.Strategy;

            return strategy.IsAuthenticated(k => (AccountPrincipal)session?[k]);
        }

        /// <summary>
        /// 是否已认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="principal">输出当前用户。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAuthenticated(this HttpSessionState session, out AccountPrincipal principal)
        {
            principal = session.ResolvePrincipal();

            return (!ReferenceEquals(principal, null) && principal.Identity.IsAuthenticated);
        }
        /// <summary>
        /// 是否已认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <param name="principal">输出当前用户。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAuthenticated(this HttpSessionStateBase session, out AccountPrincipal principal)
        {
            principal = session.ResolvePrincipal();

            return (!ReferenceEquals(principal, null) && principal.Identity.IsAuthenticated);
        }

    }
}
