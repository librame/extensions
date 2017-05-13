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
            var stategy = LibrameArchitecture.AdapterManager.Authorization.Strategy;

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
            var stategy = LibrameArchitecture.AdapterManager.Authorization.Strategy;

            // SignIn
            return stategy.SignIn(name, passwd, isPersistent,
                (k, p) => context?.Session?.Add(k, p));
        }

        /// <summary>
        /// 用户登出。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <returns>返回用户票根。</returns>
        public static AuthenticateTicket SignOut(this HttpSessionState session)
        {
            var stategy = LibrameArchitecture.AdapterManager.Authorization.Strategy;

            return stategy.SignOut((k) =>
            {
                var ticket = session.ResolveTicket();

                session?.Remove(k);

                return ticket;
            });
        }
        /// <summary>
        /// 用户登出。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <returns>返回用户票根。</returns>
        public static AuthenticateTicket SignOut(this HttpSessionStateBase session)
        {
            var stategy = LibrameArchitecture.AdapterManager.Authorization.Strategy;

            return stategy.SignOut((k) =>
            {
                var ticket = session.ResolveTicket();

                session?.Remove(k);

                return ticket;
            });
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
            var strategy = LibrameArchitecture.AdapterManager.Authorization.Strategy;

            return strategy.IsAuthenticated(k => (AccountPrincipal)session?[k]);
        }
        /// <summary>
        /// 是否已认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static bool IsAuthenticated(this HttpSessionStateBase session)
        {
            var strategy = LibrameArchitecture.AdapterManager.Authorization.Strategy;

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


        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionState"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static void OnAuthentication(this HttpSessionState session)
        {
            var strategy = LibrameArchitecture.AdapterManager.Authorization.Strategy;

            strategy.OnAuthentication(k => (AccountPrincipal)session?[k]);
        }
        /// <summary>
        /// 开始认证。
        /// </summary>
        /// <param name="session">给定的 <see cref="HttpSessionStateBase"/>。</param>
        /// <returns>返回布尔值。</returns>
        public static void OnAuthentication(this HttpSessionStateBase session)
        {
            var strategy = LibrameArchitecture.AdapterManager.Authorization.Strategy;

            strategy.OnAuthentication(k => (AccountPrincipal)session?[k]);
        }

    }
}
