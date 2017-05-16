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
        /// <param name="account">给定的帐户描述符。</param>
        /// <param name="token">给定的令牌。</param>
        /// <param name="issueDate">给定的签发日期。</param>
        /// <param name="isPersistent">给定是否持久化存储（可选；默认使用）。</param>
        /// <param name="expirationFactory">给定的过期时间方法（可选；默认为配置的过期天数过失效）。</param>
        /// <param name="userData">给定的自定义数据（可选；默认空字符串）。</param>
        /// <param name="path">给定的 Cookie 路径（可选；默认为 <see cref="FormsAuthentication.FormsCookiePath"/>）。</param>
        /// <param name="version">给定的 Cookie 版本（可选）。</param>
        public AuthenticateTicket(IAccountDescriptor account, string token, DateTime issueDate,
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

            Account = account.NotNull(nameof(account));
            Token = token.NotEmpty(nameof(token));
            Name = account.Name;
            IssueDate = issueDate;
            Expiration = expirationFactory.Invoke(issueDate);
            Expired = (DateTime.Now > Expiration);
            IsPersistent = isPersistent;
            UserData = userData ?? string.Empty;
            Path = path ?? FormsAuthentication.FormsCookiePath;
            Version = version;
        }


        /// <summary>
        /// 获取帐户。
        /// </summary>
        public IAccountDescriptor Account { get; set; }

        /// <summary>
        /// 获取令牌。
        /// </summary>
        public string Token { get; set; }
        

        /// <summary>
        /// 获取到期时间。
        /// </summary>
        public DateTime Expiration { get; set; }

        /// <summary>
        /// 获取签发日期。
        /// </summary>
        public DateTime IssueDate { get; set; }

        /// <summary>
        /// 获取是否过期。
        /// </summary>
        public bool Expired { get; set; }

        /// <summary>
        /// 获取是否持久化存储。
        /// </summary>
        public bool IsPersistent { get; set; }

        /// <summary>
        /// 获取名称。
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 获取自定义数据。
        /// </summary>
        public string UserData { get; set; }

        /// <summary>
        /// 获取 Cookie 路径。
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// 获取 Cookie 版本。
        /// </summary>
        public int Version { get; set; }

    }
}
