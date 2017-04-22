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

namespace Librame.Authorization.Descriptors
{
    /// <summary>
    /// 帐户描述符。
    /// </summary>
    [Serializable]
    public class AccountDescriptor : IAccountDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="AccountDescriptor"/> 实例。
        /// </summary>
        /// <param name="name">给定的名称。</param>
        /// <param name="passwd">给定的密码。</param>
        /// <param name="status">给定的帐户状态。</param>
        public AccountDescriptor(string name, string passwd, AccountStatus status)
        {
            Name = name;
            Passwd = passwd;
            Status = status;
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

    }
}
