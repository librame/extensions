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

namespace Librame.Authorization
{
    using Descriptors;

    /// <summary>
    /// 身份认证信息。
    /// </summary>
    public class AuthenticateInfo
    {
        /// <summary>
        /// 帐户实体。
        /// </summary>
        public IAccountDescriptor Account { get; }
        
        /// <summary>
        /// 提示消息。
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// 鉴权状态。
        /// </summary>
        public AuthenticateStatus Status { get; }

        /// <summary>
        /// 异常。
        /// </summary>
        public Exception Exception { get; }


        /// <summary>
        /// 构造一个 <see cref="AuthenticateInfo"/> 实例。
        /// </summary>
        /// <param name="account">给定的帐户实体。</param>
        /// <param name="message">给定的提示消息。</param>
        /// <param name="status">给定的鉴权状态（可选；默认成功，以便测试）。</param>
        /// <param name="exception">给定的异常（可选）。</param>
        public AuthenticateInfo(IAccountDescriptor account, string message,
            AuthenticateStatus status = AuthenticateStatus.Success, Exception exception = null)
        {
            Account = account;
            Message = message;
            Status = status;
            Exception = exception;
        }

    }
}
