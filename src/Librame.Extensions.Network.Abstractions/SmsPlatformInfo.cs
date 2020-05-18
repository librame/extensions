#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network
{
    /// <summary>
    /// 短信服务平台信息。
    /// </summary>
    public class SmsPlatformInfo
    {
        /// <summary>
        /// 帐户标识。
        /// </summary>
        public string AccountId { get; set; }

        /// <summary>
        /// 帐户密钥。
        /// </summary>
        public string AccountKey { get; set; }
    }
}
