using System;

namespace EmailService
{
    public class MailOptions
    {
        public string SmtpHost { get; }
        public string SmtpUserName { get; }
        public string SmtpPassword { get; }
        public int SmtpPort { get; }
        public bool UseSsl { get; }

        public string SenderName { get; }
        public string SenderEmail { get; }

        public MailOptions(
            string smtpHost, 
            string smtpUserName, 
            string smtpPassword,
            int smtpPort,
            bool useSsl,
            string senderName,
            string senderEmail)
        {
            if (string.IsNullOrEmpty(smtpHost) || string.IsNullOrEmpty(smtpUserName) || string.IsNullOrEmpty(smtpPassword)
                || string.IsNullOrEmpty(senderName) || string.IsNullOrEmpty(senderEmail))
            {
                throw new Exception("Configuration error");
            }
            SmtpHost = smtpHost;
            SmtpUserName = smtpUserName;
            SmtpPassword = smtpPassword;
            SmtpPort = smtpPort;
            UseSsl = useSsl;

            SenderName = senderName;
            SenderEmail = senderEmail;
        }
    }
}
