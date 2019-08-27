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
using System.Threading.Tasks;

namespace Librame.Extensions.Network
{
    using Core;

    class EmailService : NetworkServiceBase, IEmailService
    {
        private readonly IByteCodecService _byteCodec;


        public EmailService(IByteCodecService byteCodec, IOptions<CoreBuilderOptions> coreOptions,
            IOptions<NetworkBuilderOptions> options, ILoggerFactory loggerFactory)
            : base(coreOptions, options, loggerFactory)
        {
            _byteCodec = byteCodec.NotNull(nameof(byteCodec));

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
            var from = new MailAddress(Options.Email.EmailAddress, Options.Email.DisplayName, Encoding);
            var to = new MailAddress(toAddress);

            using (var message = new MailMessage(from, to))
            {
                Logger.LogDebug($"Create mail message: from={Options.Email.EmailAddress}, to={toAddress}, encoding={Encoding.AsName()}");

                message.Body = _byteCodec.EncodeString(body, Options.Email.EnableCodec);
                message.BodyEncoding = Encoding;

                message.Subject = _byteCodec.EncodeString(subject, Options.Email.EnableCodec);
                message.SubjectEncoding = Encoding;

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

    }
}