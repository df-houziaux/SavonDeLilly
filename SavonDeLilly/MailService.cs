using System.Net;
using System.Net.Mail;

namespace SavonDeLilly
{
    public class MailService(IConfiguration configuration)
    {
        private readonly IConfiguration _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        private MailMessage? _mailMessage;

        public MailService CreateMail(string from, string to, string subject, string body,
            List<string>? ccs = null, List<Attachment>? attachments = null)
        {
            _mailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            _mailMessage.To.Add(new MailAddress(to));

            if (ccs != null)
            {
                foreach (var cc in ccs)
                {
                    _mailMessage.CC.Add(new MailAddress(cc));
                }
            }

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    _mailMessage.Attachments.Add(attachment);
                }
            }

            return this;
        }

        public MailService CreateMail(string from, List<string> toAddresses, List<string> ccAddresses,
            string subject, string body, List<Attachment>? attachments = null)
        {
            _mailMessage = new MailMessage
            {
                From = new MailAddress(from),
                Subject = subject,
                Body = body,
                IsBodyHtml = true
            };

            foreach (var to in toAddresses)
            {
                _mailMessage.To.Add(new MailAddress(to));
            }

            if (ccAddresses != null)
            {
                foreach (var cc in ccAddresses)
                {
                    _mailMessage.CC.Add(new MailAddress(cc));
                }
            }

            if (attachments != null)
            {
                foreach (var attachment in attachments)
                {
                    _mailMessage.Attachments.Add(attachment);
                }
            }

            return this;
        }

        public MailService SetReplyTo(string email)
        {
            _mailMessage?.ReplyToList.Add(new MailAddress(email));
            return this;
        }

        public void Send()
        {
            if (_mailMessage == null)
            {
                throw new InvalidOperationException("No mail message has been created. Call CreateMail() first.");
            }

            string? smtpServer = _configuration["EmailSettings:SmtpServer"];
            string? smtpPortString = _configuration["EmailSettings:SmtpPort"];
            string? senderEmail = _configuration["EmailSettings:SenderEmail"];
            string? senderPassword = _configuration["EmailSettings:SenderPassword"];

            if (string.IsNullOrWhiteSpace(smtpServer) ||
                string.IsNullOrWhiteSpace(smtpPortString) ||
                string.IsNullOrWhiteSpace(senderEmail) ||
                string.IsNullOrWhiteSpace(senderPassword))
            {
                throw new InvalidOperationException("Missing required email configuration values.");
            }

            if (!int.TryParse(smtpPortString, out int smtpPort))
            {
                throw new InvalidOperationException("Invalid SMTP port configuration.");
            }

            using var smtpClient = new SmtpClient(smtpServer, smtpPort)
            {
                EnableSsl = true,
                Credentials = new NetworkCredential(senderEmail, senderPassword)
            };

            try
            {
                smtpClient.Send(_mailMessage);
            }
            catch (Exception ex)
            {
                throw new Exception("Failed to send email", ex);
            }
        }

    }
}