#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Polly;
using System;
using System.Net;

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 网络构建器选项。
    /// </summary>
    public class NetworkBuilderOptions : IExtensionBuilderOptions
    {
        /// <summary>
        /// 缓冲区大小。
        /// </summary>
        public int BufferSize { get; set; } = 1024;


        /// <summary>
        /// 字节编解码。
        /// </summary>
        public ByteCodecOptions ByteCodec { get; set; }
            = new ByteCodecOptions();

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
        public EmailOptions Email { get; set; }
            = new EmailOptions();

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


    /// <summary>
    /// 字节编解码选项。
    /// </summary>
    public class ByteCodecOptions
    {
        /// <summary>
        /// 解码工厂方法。
        /// </summary>
        public Func<ServiceFactory, byte[], byte[]> DecodeFactory { get; set; }

        /// <summary>
        /// 编码工厂方法。
        /// </summary>
        public Func<ServiceFactory, byte[], byte[]> EncodeFactory { get; set; }
    }


    /// <summary>
    /// 抓取器选项。
    /// </summary>
    public class CrawlerOptions
    {
        /// <summary>
        /// 图像文件扩展名集合（以英文逗号分隔）。
        /// </summary>
        public string ImageExtensions { get; set; }
            = ".jpg,.jpeg,.png,.bmp";

        /// <summary>
        /// 缓存过期秒数（默认 3600 秒后过期，即相同 URL 与提交数据在一小时内不会重复发起请求）。
        /// </summary>
        public int CacheExpirationSeconds { get; set; }
            = 3600;
    }


    /// <summary>
    /// 请求程序选项。
    /// </summary>
    public class RequesterOptions
    {
        /// <summary>
        /// 连接次数（默认 10 次）。
        /// </summary>
        public int ConnectionLimit { get; set; }
            = 10;

        /// <summary>
        /// 重试次数（默认 3 次）。
        /// </summary>
        public int RetryCount { get; set; }
            = 3;

        /// <summary>
        /// 重试暂停间隔的工厂方法（默认等待 1 秒后重试）。
        /// </summary>
        public Func<int, Context, TimeSpan> SleepDurationFactory { get; set; }
            = (retryCount, context) => TimeSpan.FromSeconds(1);

        /// <summary>
        /// 超时（默认 7 秒）。
        /// </summary>
        public TimeSpan Timeout { get; set; }
            = TimeSpan.FromSeconds(7);

        /// <summary>
        /// 浏览器代理。
        /// </summary>
        public string UserAgent { get; set; }
            = "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/75.0.3770.100 Safari/537.36";
    }


    /// <summary>
    /// 邮件选项。
    /// </summary>
    public class EmailOptions
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
        public Func<SmsPlatformInfo, ShortMessageDescriptor, string> GetewayUrlFactory { get; set; }
            = (info, descr) => $"https://sms.contoso.com/?id={info.AccountId}&key={info.AccountKey}&mobile={descr.Mobile}&text={descr.Text}";
    }


    /// <summary>
    /// SMTP 选项。
    /// </summary>
    public class SmtpOptions
    {
        /// <summary>
        /// 服务器。
        /// </summary>
        public string Server { get; set; }
            = "smtp.contoso.com";

        /// <summary>
        /// 端口。
        /// </summary>
        public int Port { get; set; }
            = 587;

        /// <summary>
        /// 网络凭据。
        /// </summary>
        public NetworkCredential Credential { get; set; }
            = new NetworkCredential("LoginName", "LoginPass");
    }

}
