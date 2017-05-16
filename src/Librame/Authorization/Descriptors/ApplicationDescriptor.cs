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
    /// 应用描述符。
    /// </summary>
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public class ApplicationDescriptor : IApplicationDescriptor
    {
        /// <summary>
        /// 构造一个 <see cref="ApplicationDescriptor"/> 实例。
        /// </summary>
        /// <param name="authId">给定的授权编号。</param>
        public ApplicationDescriptor(string authId)
        {
            AuthId = authId.NotEmpty(nameof(authId));
        }


        /// <summary>
        /// 授权编号。
        /// </summary>
        [DisplayName("授权编号")]
        public virtual string AuthId { get; set; }

    }
}
