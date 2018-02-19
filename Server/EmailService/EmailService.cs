using System;
using MimeKit;
using MailKit.Net.Smtp;
using System.Threading.Tasks;
using Servises.Interfaces;

namespace EmailService
{
    public class EmailService : IEmailService
    {
        private static MailOptions _mailOptions;

        public static void SetMailOptions(MailOptions mailOptions)
        {
            if(_mailOptions != null)
                throw new Exception("The configuration is already installed");

            _mailOptions = mailOptions;
        }

        public async Task SendEmailAsync(string email, string subject, string message)
        {
            var emailMessage = new MimeMessage();

            emailMessage.From.Add(new MailboxAddress(_mailOptions.SenderName, _mailOptions.SenderEmail));
            emailMessage.To.Add(new MailboxAddress("", email));
            emailMessage.Subject = subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
            {
                Text = message
            };

            using (var client = new SmtpClient())
            {
                await client.ConnectAsync(_mailOptions.SmtpHost, _mailOptions.SmtpPort, _mailOptions.UseSsl);
                await client.AuthenticateAsync(_mailOptions.SmtpUserName, _mailOptions.SmtpPassword);
                await client.SendAsync(emailMessage);

                await client.DisconnectAsync(true);
            }
        }
    }
}
