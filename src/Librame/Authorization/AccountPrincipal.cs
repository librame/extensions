#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.Security.Principal;

namespace Librame.Authorization
{
    /// <summary>
    /// 帐户用户。
    /// </summary>
    public class AccountPrincipal : GenericPrincipal, IPrincipal
    {
        /// <summary>
        /// 构造一个 <see cref="AccountPrincipal"/> 实例。
        /// </summary>
        /// <param name="identity">给定的 <see cref="IIdentity"/>。</param>
        /// <param name="roles">给定当前用户对应的角色集合。</param>
        public AccountPrincipal(IIdentity identity, string[] roles)
            : base(identity, roles)
        {
        }

    }


    /// <summary>
    /// <see cref="AccountPrincipal"/> 静态扩展。
    /// </summary>
    public static class AccountPrincipalExtensions
    {
        /// <summary>
        /// 得到用户中包含的票根。
        /// </summary>
        /// <param name="principal">给定的用户。</param>
        /// <returns>返回认证票根。</returns>
        public static AuthenticateTicket AsTicket(this IPrincipal principal)
        {
            if (principal == null || !(principal is AccountPrincipal))
                return null;

            // 取得票根
            return (principal.Identity as AccountIdentity).Ticket;
        }

        /// <summary>
        /// 得到用户中包含的票根。
        /// </summary>
        /// <param name="principal">给定的用户。</param>
        /// <returns>返回认证票根。</returns>
        public static AuthenticateTicket AsTicket(this AccountPrincipal principal)
        {
            if (principal == null)
                return null;

            // 取得票根
            return (principal.Identity as AccountIdentity).Ticket;
        }

    }
}
