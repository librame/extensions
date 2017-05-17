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
using System.Runtime.InteropServices;
using System.Web.Security;

namespace Librame.Authorization
{
    using Descriptors;
    using Utility;

    /// <summary>
    /// 身份认证票根。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public sealed class AuthenticateTicket
    {
        /// <summary>
        /// 构造一个 <see cref="AuthenticateTicket"/> 实例。
        /// </summary>
        public AuthenticateTicket()
        {
        }
        /// <summary>
        /// 构造一个 <see cref="AuthenticateTicket"/> 实例。
        /// </summary>
        /// <param name="accountId">给定的帐户编号。</param>
        /// <param name="appId">给定的应用编号。</param>
        /// <param name="name">给定的用户名。</param>
        /// <param name="token">给定的令牌。</param>
        /// <param name="issueDate">给定的签发日期。</param>
        /// <param name="isPersistent">给定是否持久化存储（可选；默认使用）。</param>
        /// <param name="expirationFactory">给定的过期时间方法（可选；默认为配置的过期天数过失效）。</param>
        /// <param name="userData">给定的自定义数据（可选；默认空字符串）。</param>
        /// <param name="path">给定的 Cookie 路径（可选；默认为 <see cref="FormsAuthentication.FormsCookiePath"/>）。</param>
        /// <param name="version">给定的 Cookie 版本（可选）。</param>
        public AuthenticateTicket(int accountId, int appId, string name, string token, DateTime issueDate,
            bool isPersistent = true,
            Func<DateTime, DateTime> expirationFactory = null,
            string userData = null,
            string path = null,
            int version = 1)
        {
            if (ReferenceEquals(expirationFactory, null))
            {
                var authSettings = LibrameArchitecture.Adapters.Authorization.AuthSettings;
                expirationFactory = dt => dt.AddDays(authSettings.ExpirationDays);
            }

            AccountId = accountId;
            AppId = appId;
            Name = name;
            Token = token.NotEmpty(nameof(token));
            IssueDate = issueDate;
            Expiration = expirationFactory.Invoke(issueDate);
            Expired = (DateTime.Now > Expiration);
            IsPersistent = isPersistent;
            UserData = userData ?? string.Empty;
            Path = path ?? FormsAuthentication.FormsCookiePath;
            Version = version;
        }
        /// <summary>
        /// 构造一个 <see cref="AuthenticateTicket"/> 实例。
        /// </summary>
        /// <param name="account">给定的帐户。</param>
        /// <param name="token">给定的令牌。</param>
        /// <param name="issueDate">给定的签发日期。</param>
        /// <param name="isPersistent">给定是否持久化存储（可选；默认使用）。</param>
        public AuthenticateTicket(IAccountDescriptor account, string token, DateTime issueDate,
            bool isPersistent = true)
            : this(account.Id, account.AppId, account.Name, token, issueDate, isPersistent)
        {
        }


        /// <summary>
        /// 帐户编号。
        /// </summary>
        public int AccountId { get; set; }

        /// <summary>
        /// 应用编号。
        /// </summary>
        public int AppId { get; set; }

        /// <summary>
        /// 名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 令牌。
        /// </summary>
        public string Token { get; set; }
        

        /// <summary>
        /// 到期时间。
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// 签发日期。
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// 是否过期。
        /// </summary>
        public bool Expired { get; set; }

        /// <summary>
        /// 是否持久化存储。
        /// </summary>
        public bool IsPersistent { get; set; }
        
        /// <summary>
        /// 自定义数据。
        /// </summary>
        public string UserData { get; set; }

        /// <summary>
        /// Cookie 路径。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// Cookie 版本。
        /// </summary>
        public int Version { get; set; }

    }
}
