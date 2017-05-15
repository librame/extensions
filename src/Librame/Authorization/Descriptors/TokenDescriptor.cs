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
    /// 令牌描述符。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class TokenDescriptor : ITokenDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="AccountDescriptor"/> 实例。
        /// </summary>
        public TokenDescriptor()
        {
        }
        /// <summary>
        /// 构造一个 <see cref="AccountDescriptor"/> 实例。
        /// </summary>
        /// <exception cref="ArgumentNullException">
        /// name or ticket is null or empty.
        /// </exception>
        /// <param name="name">给定的名称。</param>
        /// <param name="ticket">给定的票根。</param>
        public TokenDescriptor(string name, string ticket)
        {
            Name = name.NotNullOrEmpty(nameof(name));
            Ticket = ticket.NotNullOrEmpty(nameof(ticket));
        }


        /// <summary>
        /// 名称。
        /// </summary>
        [DisplayName("令牌编号")]
        public virtual string Name { get; set; }

        /// <summary>
        /// 票根。
        /// </summary>
        [DisplayName("令牌票根")]
        public virtual string Ticket { get; set; }

    }
}
