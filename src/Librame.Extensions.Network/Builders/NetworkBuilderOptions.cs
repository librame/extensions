﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using System;
using System.Net;

namespace Librame.Extensions.Network
{
    using Core;

    /// <summary>
    /// 网络构建器选项。
    /// </summary>
    public class NetworkBuilderOptions : AbstractBuilderOptions
    {
        /// <summary>
        /// 字节编解码选项。
        /// </summary>
        public ByteCodecOptions ByteCodec { get; set; }
            = new ByteCodecOptions();

        /// <summary>
        /// 抓取器选项。
        /// </summary>
        public CrawlerOptions Crawler { get; set; }
            = new CrawlerOptions();

        /// <summary>
        /// 请求选项。
        /// </summary>
        public RequestOptions Request { get; set; }
            = new RequestOptions();

        /// <summary>
        /// 邮件选项。
        /// </summary>
        public EmailOptions Email { get; set; }
            = new EmailOptions();

        /// <summary>
        /// 短信选项。
        /// </summary>
        public SmsOptions Sms { get; set; }
            = new SmsOptions();

        /// <summary>
        /// SMTP 选项。
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
        public Func<IServiceProvider, byte[], byte[]> DecodeFactory { get; set; }

        /// <summary>
        /// 编码工厂方法。
        /// </summary>
        public Func<IServiceProvider, byte[], byte[]> EncodeFactory { get; set; }
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
    }


    /// <summary>
    /// 请求程序选项。
    /// </summary>
    public class RequestOptions
    {
        /// <summary>
        /// 允许自动重定向。
        /// </summary>
        public bool AllowAutoRedirect { get; set; }
            = true;

        /// <summary>
        /// 连接次数。
        /// </summary>
        public int ConnectionLimit { get; set; }
            = 10;

        /// <summary>
        /// 引用页。
        /// </summary>
        public string Referer { get; set; }
            = string.Empty;

        /// <summary>
        /// 超时。
        /// </summary>
        public int Timeout { get; set; }
            = 10000;

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
        /// 启用编解码。
        /// </summary>
        public bool EnableCodec { get; set; }
            = false;

        /// <summary>
        /// 在接收到来自服务器的 100 次连续响应之前要等待的超时（以毫秒为单位，默认为 10 秒）。
        /// </summary>
        public int ContinueTimeout { get; set; }
            = 10000;

        /// <summary>
        /// 网关链接。
        /// </summary>
        public string GetewayUrl { get; set; }
            = "https://sms.contoso.com";
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
