#region License

/* **************************************************************************************
 * Copyright (c) Librame Pong All rights reserved.
 * 
 * https://github.com/librame
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;

namespace Librame.Extensions.Network.Options
{
    /// <summary>
    /// 短信服务选项。
    /// </summary>
    public class SmsOptions
    {
        /// <summary>
        /// 启用编解码（默认禁用）。
        /// </summary>
        public bool EnableCodec { get; set; }
            = false;

        /// <summary>
        /// 平台信息。
        /// </summary>
        public SmsPlatformInfo PlatformInfo { get; set; }
            = new SmsPlatformInfo();

        /// <summary>
        /// 网关链接（传入参数依次为）。
        /// </summary>
        public Func<SmsPlatformInfo, SmsDescriptor, string> GetewayUrlFactory { get; set; }
            = (info, descr) => $"https://sms.contoso.com/?id={info.AccountId}&key={info.AccountKey}&mobile={descr.Mobile}&text={descr.Text}";
    }
}
