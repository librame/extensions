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
using System.Net.Mail;
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    /// <summary>
    /// 邮箱服务发送器接口。
    /// </summary>
    public interface IEmailSender : INetworkService
    {
        /// <summary>
        /// 发送完成后的回调方法。
        /// </summary>
        Action<object, AsyncCompletedEventArgs> SendCompletedCallback { get; set; }


        /// <summary>
        /// 创建附件。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="Attachment"/>。</returns>
        Attachment CreateAttachment(string fileName);


        /// <summary>
        /// 发送邮件。
        /// </summary>
        /// <param name="toAddress">给定的邮箱接收地址。</param>
        /// <param name="subject">给定的主题。</param>
        /// <param name="body">给定的内容。</param>
        /// <param name="configureMessage">给定的邮箱信息配置方法（可选）。</param>
        /// <param name="configureClient">给定的 SMTP 客户端配置方法（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        Task SendAsync(string toAddress, string subject, string body,
            Action<MailMessage> configureMessage = null,
            Action<SmtpClient> configureClient = null);
    }
}
