using CandidateCodeTest.Services.Interfaces;
using System;
using System.Net;
using System.Net.Mail;

namespace CandidateCodeTest.Services.Implementation
{
    internal class EmailMessageService : IEmailService
    {
        private readonly ILogger _logger;

        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _fromEmail;
        private readonly string _emailPassword;

        public EmailMessageService(ILogger logger, string smtpServer, int smtpPort, string fromEmail, string emailPassword)
        {
            _logger = logger;
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _fromEmail = fromEmail;
            _emailPassword = emailPassword;
        }

        public void SendEmail(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_smtpServer)
                {
                    Port = _smtpPort,
                    Credentials = new NetworkCredential(_fromEmail, _emailPassword),
                    EnableSsl = true
                };

                var mailMessage = new MailMessage
                {
                    From = new MailAddress(_fromEmail),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                mailMessage.To.Add(toEmail);

                smtpClient.Send(mailMessage);
                _logger.Log("Email sent successfully to " + toEmail);
            }
            catch (Exception ex)
            {
                _logger.Log("Error sending email: " + ex.Message);
            }
        }
    }
}
