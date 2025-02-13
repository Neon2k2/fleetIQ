using System;
using System.Net;
using System.Net.Mail;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace AuthenticationService.Infrastructure.Services
{
    public class EmailService
    {
        private readonly IConfiguration _config;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IConfiguration config, ILogger<EmailService> logger)
        {
            _config = config;
            _logger = logger;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient
                {
                    Host = _config["EmailSettings:SmtpHost"],
                    Port = int.Parse(_config["EmailSettings:SmtpPort"]),
                    EnableSsl = bool.Parse(_config["EmailSettings:EnableSsl"]),
                    Credentials = new NetworkCredential(
                        _config["EmailSettings:Username"],
                        _config["EmailSettings:Password"])
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_config["EmailSettings:FromEmail"]),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };
                mailMessage.To.Add(to);

                await smtpClient.SendMailAsync(mailMessage);

                _logger.LogInformation($"Email sent to {to}");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error sending email to {to}");
                return false;
            }
        }
    }
}
