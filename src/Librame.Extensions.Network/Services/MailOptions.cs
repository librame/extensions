#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.Services
{
    /// <summary>
    /// 邮件选项。
    /// </summary>
    public class MailOptions
    {
        /// <summary>
        /// 启用编解码。
        /// </summary>
        public bool EnableCodec { get; set; }
            = false;

        /// <summary>
        /// 邮箱地址。
        /// </summary>
        public string EmailAddress { get; set; }
            = "myemail@contoso.com";

        /// <summary>
        /// 显示名称。
        /// </summary>
        public string DisplayName { get; set; }
            = "MyEmail";
    }
}
