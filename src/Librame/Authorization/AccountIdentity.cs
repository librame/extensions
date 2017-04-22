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
using System.Web.Security;

namespace Librame.Authorization
{
    using Utility;

    /// <summary>
    /// 帐户标识。
    /// </summary>
    public class AccountIdentity : GenericIdentity, IIdentity
    {
        /// <summary>
        /// 获取票根。
        /// </summary>
        public FormsAuthenticationTicket Ticket { get; }

        /// <summary>
        /// 构造一个 <see cref="AccountIdentity"/> 实例。
        /// </summary>
        /// <param name="ticket">给定的 <see cref="FormsAuthenticationTicket"/>。</param>
        public AccountIdentity(FormsAuthenticationTicket ticket)
            : base(ticket.Name)
        {
            Ticket = ticket.NotNull(nameof(ticket));
        }


        /// <summary>
        /// 获取所使用的身份验证的类型。
        /// </summary>
        public override string AuthenticationType => AuthorizeHelper.AUTHORIZATION_TYPE;

        /// <summary>
        /// 获取一个值，该值指示是否验证了用户。
        /// </summary>
        public override bool IsAuthenticated => (Name.Length > 0 && !Ticket.Expired);

        /// <summary>
        /// 获取当前用户的名称。
        /// </summary>
        public override string Name => Ticket?.Name;

    }
}
