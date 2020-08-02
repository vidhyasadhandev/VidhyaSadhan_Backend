using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using VidyaSadhan_API.Models;

namespace VidyaSadhan_API.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly EmailSettings _emailSettings;
        private readonly IWebHostEnvironment _environment;

        public EmailSender(IOptions<EmailSettings> emailSettings, IWebHostEnvironment environment)
        {
            _environment = environment;
            _emailSettings = emailSettings.Value;
        }

        public async Task SendEmailAsync(EmailMessage message)
        {
            try
            {
                var mimeMessage = new MimeMessage();

                mimeMessage.From.Add(new MailboxAddress(_emailSettings.SenderName, _emailSettings.Sender));
                mimeMessage.To.AddRange(message.Email.Select(x => MailboxAddress.Parse(x)));

                mimeMessage.Subject = message.Subject;

                mimeMessage.Body = new TextPart("html")
                {
                    Text = message.Message
                };

                using (var client = new SmtpClient())
                {
                    client.MessageSent += (sender, args) => { };

                    client.ServerCertificateValidationCallback = (s, c, h, e) => true;
                    

                    if (_environment.IsDevelopment())
                    {
                        await client.ConnectAsync(_emailSettings.Server, _emailSettings.Port,true);
                    }
                    else
                    {
                        await client.ConnectAsync(_emailSettings.Server);
                    }

                    await client.AuthenticateAsync(_emailSettings.Sender, "dxexodscuusnnaic");

                    await client.SendAsync(mimeMessage);

                    await client.DisconnectAsync(true);
                }
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }
        }
    }
}
