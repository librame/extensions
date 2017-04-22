#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System.ComponentModel;

namespace Librame.Authorization
{
    /// <summary>
    /// 身份认证状态。
    /// </summary>
    [Description("身份认证状态")]
    public enum AuthenticateStatus
    {
        /// <summary>
        /// 默认。
        /// </summary>
        [Description("默认")]
        Default = 0,

        /// <summary>
        /// 成功。
        /// </summary>
        [Description("成功")]
        Success = 1,

        /// <summary>
        /// 帐号为空。
        /// </summary>
        [Description("帐号为空")]
        NameIsEmpty = 2,

        /// <summary>
        /// 帐号错误。
        /// </summary>
        [Description("帐号错误")]
        NameError = 4,

        /// <summary>
        /// 邮箱错误。
        /// </summary>
        [Description("邮箱错误")]
        EmailError = 8,

        /// <summary>
        /// 手机错误。
        /// </summary>
        [Description("手机错误")]
        PhoneError = 16,

        /// <summary>
        /// 身份证号错误。
        /// </summary>
        [Description("身份证号错误")]
        IdCardError = 32,

        /// <summary>
        /// 密码错误。
        /// </summary>
        [Description("密码错误")]
        PasswdError = 64,

        /// <summary>
        /// 状态错误。
        /// </summary>
        [Description("状态错误")]
        StatusError = 128,

        /// <summary>
        /// 系统错误。
        /// </summary>
        [Description("系统错误")]
        SystemError = 256,

        /// <summary>
        /// 不被支持。
        /// </summary>
        [Description("不被支持")]
        NoSupport = 512,

        /// <summary>
        /// 帐号已登陆。
        /// </summary>
        [Description("帐号已登陆")]
        NameLogined = 1024,
        
        /// <summary>
        /// 未知。
        /// </summary>
        [Description("未知")]
        Unknown = 2048
    }
}
