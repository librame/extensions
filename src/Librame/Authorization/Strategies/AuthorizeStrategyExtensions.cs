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
using System.Security.Principal;
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
        /// 用户登录。
        /// </summary>
        /// <param name="context">给定的 <see cref="HttpContext"/>。</param>
        /// <param name="name">给定的用户名。</param>
        /// <param name="passwd">给定的密码。</param>
        /// <param name="isPersistent">是否持久化存储。</param>
        /// <returns>返回认证信息。</returns>
        public static AuthenticateInfo SignIn(this HttpContext context, string name, string passwd, bool isPersistent)
        {
            var stategy = LibrameArchitecture.AdapterManager.AuthorizationAdapter.Strategy;

            // SignIn
            return stategy.SignIn(name, passwd, isPersistent,
                (k, p) => context?.Session?.Add(k, p));
        }
        /// <summary>
        /// 用户登录。
        /// </summary>
        /// <param name="context">给定的 <see cref="HttpContextBase"/>。</param>
        /// <param name="name">给定的用户名。</param>
        /// <param name="passwd">给定的密码。</param>
        /// <param name="isPersistent">是否持久化存储。</param>
        /// <returns>返回认证信息。</returns>
        public static AuthenticateInfo SignIn(this HttpContextBase context, string name, string passwd, bool isPersistent)
        {
            var stategy = LibrameArchitecture.AdapterManager.AuthorizationAdapter.Strategy;

            // SignIn
            return stategy.SignIn(name, passwd, isPersistent,
                (k, p) => context?.Session?.Add(k, p));
        }

        /// <summary>
        /// 用户登出。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        public static void SignOut(this HttpSessionState session)
        {
            var stategy = LibrameArchitecture.AdapterManager.AuthorizationAdapter.Strategy;

            stategy.SignOut((k) => session?.Remove(k));
        }
        /// <summary>
        /// 用户登出。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        public static void SignOut(this HttpSessionStateBase session)
        {
            var stategy = LibrameArchitecture.AdapterManager.AuthorizationAdapter.Strategy;

            stategy.SignOut((k) => session?.Remove(k));
        }


        /// <summary>
        /// 解析用户信息。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <returns>返回 <see cref="IPrincipal"/>。</returns>
        public static IPrincipal ResolvePrincipal(this HttpSessionState session)
        {
            return (IPrincipal)session?[AuthorizeHelper.AUTHORIZATION_KEY];
        }
        /// <summary>
        /// 解析用户信息。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <returns>返回 <see cref="IPrincipal"/>。</returns>
        public static IPrincipal ResolvePrincipal(this HttpSessionStateBase session)
        {
            return (IPrincipal)session?[AuthorizeHelper.AUTHORIZATION_KEY];
        }


        /// <summary>
        /// 如同用户中包含的票根。
        /// </summary>
        /// <param name="principal">给定的用户。</param>
        /// <returns>返回窗体认证票根。</returns>
        public static FormsAuthenticationTicket AsTicket(this IPrincipal principal)
        {
            if (!(principal is AccountPrincipal))
                return null;

            // 取得票根
            return (principal.Identity as AccountIdentity).Ticket;
        }

        /// <summary>
        /// 如同用户中包含的票根数据描述符。
        /// </summary>
        /// <param name="principal">给定的用户。</param>
        /// <returns>返回票根数据描述符。</returns>
        public static TicketDataDescriptor AsTicketData(this IPrincipal principal)
        {
            // 取得票根用户数据
            var descriptor = principal.AsTicket()?.UserData;

            return TicketDataDescriptor.Deserialize(descriptor);
        }


        /// <summary>
        /// 是否已认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAuthenticated(this HttpSessionState session)
        {
            var strategy = LibrameArchitecture.AdapterManager.AuthorizationAdapter.Strategy;

            return strategy.IsAuthenticated(k => (IPrincipal)session?[k]);
        }
        /// <summary>
        /// 是否已认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAuthenticated(this HttpSessionStateBase session)
        {
            var strategy = LibrameArchitecture.AdapterManager.AuthorizationAdapter.Strategy;

            return strategy.IsAuthenticated(k => (IPrincipal)session?[k]);
        }

        /// <summary>
        /// 是否已认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <param name="principal">输出当前用户。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAuthenticated(this HttpSessionState session, out IPrincipal principal)
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
        public static bool IsAuthenticated(this HttpSessionStateBase session, out IPrincipal principal)
        {
            principal = session.ResolvePrincipal();

            return (!ReferenceEquals(principal, null) && principal.Identity.IsAuthenticated);
        }


        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static void OnAuthentication(this HttpSessionState session)
        {
            var strategy = LibrameArchitecture.AdapterManager.AuthorizationAdapter.Strategy;

            strategy.OnAuthentication(k => (IPrincipal)session?[k]);
        }
        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static void OnAuthentication(this HttpSessionStateBase session)
        {
            var strategy = LibrameArchitecture.AdapterManager.AuthorizationAdapter.Strategy;

            strategy.OnAuthentication(k => (IPrincipal)session?[k]);
        }

    }
}
