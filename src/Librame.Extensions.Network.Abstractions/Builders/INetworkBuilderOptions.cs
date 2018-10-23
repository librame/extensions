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
    using Encryption;

    /// <summary>
    /// 网络构建器选项接口。
    /// </summary>
    public interface INetworkBuilderOptions : IEncryptionBuilderOptions
    {
        /// <summary>
        /// 抓取器选项。
        /// </summary>
        CrawlerOptions Crawler { get; set; }

        /// <summary>
        /// 邮件选项。
        /// </summary>
        EmailOptions Email { get; set; }

        /// <summary>
        /// 短信选项。
        /// </summary>
        SmsOptions Sms { get; set; }

        /// <summary>
        /// 服务器选项。
        /// </summary>
        ServerOptions Server { get; set; }
    }
}
