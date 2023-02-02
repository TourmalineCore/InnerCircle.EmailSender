﻿using EmailSender.Application.Services.Options;
using Microsoft.Extensions.Options;
using EmailSender.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;

namespace EmailSender.Application.Services
{
    public class SendGridEmailService : IEmailSender
    {
        private readonly SendGridOptions _mailOptions;
        private readonly SendGridClient _client;

        public SendGridEmailService(IOptions<SendGridOptions> mailOptions)
        {
            _mailOptions = mailOptions.Value;
            _client = new SendGridClient(_mailOptions.SendGridAPIKey);
        }

        public async Task SendEmailAsync(MailModel mailModel)
        {
            var msg = new SendGridMessage()
            {
                From = new EmailAddress(_mailOptions.SenderEmail, _mailOptions.SenderName),
                Subject = mailModel.Subject,
                PlainTextContent = mailModel.Body,
                HtmlContent = $"<div>{mailModel.Body}</div>",
            };

            msg.AddTo(mailModel.To);

            await _client.SendEmailAsync(msg);
        }
    }
}