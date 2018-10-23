﻿#region License

/* **************************************************************************************
 * Copyright (c) Librame Pang All rights reserved.
 * 
 * http://librame.net
 * 
 * You must not remove this notice, or any other, from this software.
 * **************************************************************************************/

#endregion

using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.ComponentModel;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;

namespace Librame.Extensions.Network
{
    using Encryption;

    /// <summary>
    /// 内部邮箱服务发送器。
    /// </summary>
    internal class InternalEmailSender : AbstractNetworkService<InternalEmailSender>, IEmailSender
    {
        /// <summary>
        /// 构造一个 <see cref="InternalEmailSender"/> 实例。
        /// </summary>
        /// <param name="hash">给定的 <see cref="IHashAlgorithmService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{MailService}"/>。</param>
        public InternalEmailSender(IHashAlgorithmService hash, IOptions<DefaultNetworkBuilderOptions> options, ILogger<InternalEmailSender> logger)
            : base(hash, options, logger)
        {
            SendCompletedCallback = (sender, e) =>
            {
                // e.UserState = userToken
                if (e.Error.IsNotDefault())
                {
                    Logger.LogError(e.Error.AsInnerMessage());
                }
            };
        }

        
        /// <summary>
        /// 发送完成后的回调方法。
        /// </summary>
        public Action<object, AsyncCompletedEventArgs> SendCompletedCallback { get; set; }


        /// <summary>
        /// 创建附件。
        /// </summary>
        /// <param name="fileName">给定的文件名。</param>
        /// <returns>返回 <see cref="Attachment"/>。</returns>
        public Attachment CreateAttachment(string fileName)
        {
            var fileInfo = new FileInfo(fileName);

            var attachment = new Attachment(fileInfo.Name, MediaTypeNames.Application.Octet);
            Logger.LogDebug($"Attachement file: {fileInfo.FullName}");

            attachment.NameEncoding = Encoding;
            attachment.ContentDisposition.CreationDate = fileInfo.CreationTime;
            attachment.ContentDisposition.ModificationDate = fileInfo.LastWriteTime;
            attachment.ContentDisposition.ReadDate = fileInfo.LastAccessTime;
            attachment.ContentDisposition.Size = fileInfo.Length;
            Logger.LogDebug($"file size: {attachment.ContentDisposition.Size}");

            return attachment;
        }


        /// <summary>
        /// 发送邮件。
        /// </summary>
        /// <param name="toAddress">给定的邮箱接收地址。</param>
        /// <param name="subject">给定的主题。</param>
        /// <param name="body">给定的内容。</param>
        /// <param name="userToken">给定的自定义对象（可选）。</param>
        /// <param name="configureMessage">给定的邮箱信息配置方法（可选）。</param>
        /// <param name="configureClient">给定的 SMTP 客户端配置方法（可选）。</param>
        public void SendAsync(string toAddress, string subject, string body, object userToken = null,
            Action<MailMessage> configureMessage = null,
            Action<SmtpClient> configureClient = null)
        {
            try
            {
                var from = new MailAddress(Options.Email.EmailAddress, Options.Email.DisplayName, Encoding);
                var to = new MailAddress(toAddress);

                using (var message = new MailMessage(from, to))
                {
                    Logger.LogDebug($"Create mail message: from={Options.Email.EmailAddress}, to={toAddress}, encoding={Encoding.AsName()}");

                    message.Body = body;
                    message.BodyEncoding = Encoding;
                    Logger.LogDebug($"Set body: {message.Body}");

                    message.Subject = subject;
                    message.SubjectEncoding = Encoding;
                    Logger.LogDebug($"Set subject: {message.Subject}");

                    // Configure MailMessage
                    configureMessage?.Invoke(message);

                    var smtp = Options.Email.Smtp;
                    using (var client = new SmtpClient(smtp.Server, smtp.Port))
                    {
                        Logger.LogDebug($"Create smtp client: server={client.Host}, port={client.Port}");

                        client.Credentials = smtp.Credential;
                        Logger.LogDebug($"Set credentials: domain={smtp.Credential.Domain}, username={smtp.Credential.UserName}, password=...");

                        client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                        // Configure SmtpClient
                        configureClient?.Invoke(client);

                        // SendAsync
                        client.SendAsync(message, userToken);
                        Logger.LogDebug($"Send mail.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex.AsInnerMessage());
            }
        }

    }
}