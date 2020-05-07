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
using System;
using System.ComponentModel;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Mail;
using System.Net.Mime;
using System.Threading.Tasks;

namespace Librame.Extensions.Network.Services
{
    using Core.Services;

    [SuppressMessage("Microsoft.Performance", "CA1812:AvoidUninstantiatedInternalClasses")]
    internal class EmailService : NetworkServiceBase, IEmailService
    {
        private readonly IByteCodecService _byteCodec;
        private readonly IClockService _clock;


        public EmailService(IByteCodecService byteCodec, IClockService clock)
            : base(byteCodec.CastTo<IByteCodecService, NetworkServiceBase>(nameof(byteCodec)))
        {
            _byteCodec = byteCodec;
            _clock = clock;

            SendCompletedCallback = (sender, e) =>
            {
                // e.UserState = userToken
                if (e.Error.IsNotNull())
                    Logger.LogError(e.Error, e.Error.AsInnerMessage());
            };
        }

        
        public Action<object, AsyncCompletedEventArgs> SendCompletedCallback { get; set; }


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


        public async Task SendAsync(string toAddress, string subject, string body,
            Action<MailMessage> configureMessage = null,
            Action<SmtpClient> configureClient = null)
        {
            var from = new MailAddress(Options.Mail.EmailAddress, Options.Mail.DisplayName, Encoding);
            var to = new MailAddress(toAddress);

            using (var message = new MailMessage(from, to))
            {
                Logger.LogDebug($"Create mail message: from={Options.Mail.EmailAddress}, to={toAddress}, encoding={Encoding.AsName()}");

                message.Body = _byteCodec.EncodeString(body, Options.Mail.EnableCodec);
                message.BodyEncoding = Encoding;

                message.Subject = _byteCodec.EncodeString(subject, Options.Mail.EnableCodec);
                message.SubjectEncoding = Encoding;

                // Configure MailMessage
                configureMessage?.Invoke(message);

                //var smtp = Options.Smtp;
                using (var client = new SmtpClient(Options.Smtp.Server, Options.Smtp.Port))
                {
                    Logger.LogDebug($"Create smtp client: server={client.Host}, port={client.Port}");

                    client.Credentials = Options.Smtp.Credential;
                    Logger.LogDebug($"Set credentials: domain={Options.Smtp.Credential.Domain}, username={Options.Smtp.Credential.UserName}");

                    client.SendCompleted += new SendCompletedEventHandler(SendCompletedCallback);

                    // Configure SmtpClient
                    configureClient?.Invoke(client);

                    // SendAsync
                    await client.SendMailAsync(message).ConfigureAndWaitAsync();

                    var timestamp = await _clock.GetOffsetNowAsync().ConfigureAndResultAsync();
                    Logger.LogDebug($"Successful send of mail at {timestamp}.");
                }
            }
        }

    }
}
