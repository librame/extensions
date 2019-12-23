#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

namespace Librame.Extensions.Network.Builders
{
    using Core.Builders;
    using Requesters;
    using Services;

    /// <summary>
    /// 网络构建器选项。
    /// </summary>
    public class NetworkBuilderOptions : IExtensionBuilderOptions
    {
        /// <summary>
        /// 缓冲区大小。
        /// </summary>
        public int BufferSize { get; set; }
            = 1024;


        /// <summary>
        /// 抓取器。
        /// </summary>
        public CrawlerOptions Crawler { get; set; }
            = new CrawlerOptions();

        /// <summary>
        /// 请求程序。
        /// </summary>
        public RequesterOptions Requester { get; set; }
            = new RequesterOptions();

        /// <summary>
        /// 邮件。
        /// </summary>
        public MailOptions Mail { get; set; }
            = new MailOptions();

        /// <summary>
        /// 短信。
        /// </summary>
        public SmsOptions Sms { get; set; }
            = new SmsOptions();

        /// <summary>
        /// SMTP。
        /// </summary>
        public SmtpOptions Smtp { get; set; }
            = new SmtpOptions();
    }
}
