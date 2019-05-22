#region License

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
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    using Encryption;

    /// <summary>
    /// 内部邮箱服务。
    /// </summary>
    internal class InternalEmailService : AbstractNetworkService<InternalEmailService>, IEmailService
    {
        /// <summary>
        /// 构造一个 <see cref="InternalEmailService"/> 实例。
        /// </summary>
        /// <param name="hash">给定的 <see cref="IHashService"/>。</param>
        /// <param name="options">给定的 <see cref="IOptions{NetworkBuilderOptions}"/>。</param>
        /// <param name="logger">给定的 <see cref="ILogger{InternalEmailService}"/>。</param>
        public InternalEmailService(IHashService hash,
            IOptions<NetworkBuilderOptions> options, ILogger<InternalEmailService> logger)
            : base(hash, options, logger)
        {
            SendCompletedCallback = (sender, e) =>
            {
                // e.UserState = userToken
                if (e.Error != null)
                    Logger.LogError(e.Error, e.Error.AsInnerMessage());
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
        /// <param name="configureMessage">给定的邮箱信息配置方法（可选）。</param>
        /// <param name="configureClient">给定的 SMTP 客户端配置方法（可选）。</param>
        /// <returns>返回一个异步操作。</returns>
        public async Task SendAsync(string toAddress, string subject, string body,
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

                    //var smtp = Options.Smtp;
                    using (var client = new SmtpClient(Options.Smtp.Server, Options.Smtp.Port))
                    {
                        Logger.LogDebug($"Create smtp client: server={client.Host}, port={client.Port}");

                        client.Credentials = Options.Smtp.Credential;
                        Logger.LogDebug($"Set credentials: domain={Options.Smtp.Credential.Domain}, username={Options.Smtp.Credential.UserName}, password=***");

                        client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                        // Configure SmtpClient
                        configureClient?.Invoke(client);

                        // SendAsync
                        await client.SendMailAsync(message);
                        Logger.LogDebug($"Send mail.");
                    }
                }
            }
            catch (Exception ex)
            {
                Logger.LogError(ex, ex.AsInnerMessage());
            }
        }

    }
}
