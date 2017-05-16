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
using System.ComponentModel;
using System.Runtime.InteropServices;

namespace Librame.Authorization.Descriptors
{
    using Utility;

    /// <summary>
    /// 帐户描述符。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class AccountDescriptor : IAccountDescriptor
    {
        /// <summary>
        /// 构造一个默认帐户描述符实例。
        /// </summary>
        public AccountDescriptor()
        {
            Status = AccountStatus.Default;
        }
        /// <summary>
        /// 构造一个帐户描述符实例。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="passwd">给定的密码。</param>
        /// <param name="status">给定的帐户状态。</param>
        public AccountDescriptor(string name, string passwd, AccountStatus status)
        {
            Name = name.NotEmpty(nameof(name));
            Passwd = passwd;
            Status = status;
        }
        /// <summary>
        /// 构造一个测试帐户描述符实例。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        internal AccountDescriptor(string name)
        {
            Name = name.NotEmpty(nameof(name));
            Passwd = AuthorizeHelper.DEFAULT_PASSWD;
            Status = AccountStatus.Active;
        }


        /// <summary>
        /// 名称。
        /// </summary>
        [DisplayName("帐户名称")]
        public virtual string Name { get; set; }

        /// <summary>
        /// 密码。
        /// </summary>
        [DisplayName("帐户密码")]
        public virtual string Passwd { get; set; }

        /// <summary>
        /// 状态。
        /// </summary>
        [DisplayName("帐户状态")]
        [DefaultValue(AccountStatus.Active)]
        public virtual AccountStatus Status { get; }


        /// <summary>
        /// 重置密码。
        /// </summary>
        /// <param name="newPasswd">给定要重置的新密码。</param>
        public virtual void ResetPasswd(string newPasswd = null)
        {
            if (newPasswd == null)
                newPasswd = string.Empty;

            Passwd = newPasswd;
        }

    }
}
