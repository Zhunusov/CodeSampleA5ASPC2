using System;
using AuthenticationServices.Jwt;
using EmailService;
using Microsoft.Extensions.Configuration;

namespace Web.Initializers
{
    public static class ConfigureService
    {

        public static void SetConfigis(IConfiguration configuration)
        {
            SetJwtConfig(configuration);
            SetMailConfig(configuration);
        }

        private static void SetJwtConfig(IConfiguration configuration)
        {
            AuthJwtOptions.SetAuthOptions(
                issuer: configuration["Tokens:Issuer"],
                audience: configuration["Tokens:Audience"],
                key: configuration["Tokens:Key"],
                lifetime: Convert.ToInt32(configuration["Tokens:Lifetime"]));
        }

        private static void SetMailConfig(IConfiguration configuration)
        {
            if (!int.TryParse(configuration["MailConfig:SmtpPort"], out var port))
            {
                throw new Exception("Incorrect configuration");
            }

            if (!bool.TryParse(configuration["MailConfig:UseSsl"], out var useSsl))
            {
                throw new Exception("Incorrect configuration");
            }

            EmailService.EmailService.SetMailOptions(
                new MailOptions(
                    smtpHost: configuration["MailConfig:SmtpHost"],
                    smtpUserName: configuration["MailConfig:SmtpUserName"],
                    smtpPassword: configuration["MailConfig:SmtpPassword"],
                    smtpPort: port,
                    useSsl: useSsl,
                    senderName: configuration["MailConfig:SenderName"],
                    senderEmail: configuration["MailConfig:SenderEmail"]
                )
            );
        }
    }
}
