using BookStore.Model;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace BookStore.Repository
{
    public class EmailRepository : IEmailRepository
    {
        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfiguration _smtpconfig;
        private readonly IConfiguration _configuration;

        public EmailRepository(IOptions<SMTPConfiguration> smtpconfig, IConfiguration configuration)
        {
            _smtpconfig = smtpconfig.Value;
            _configuration = configuration;
        }
        public async Task SendEmailMessage(EmailMessage emailMessage)
        {
            emailMessage.Subject = "Email Message from BookStore";
            emailMessage.Body = UpdatePlaceHolders(GetEmailBody("TestEmail"), emailMessage.PlaceHolders);
            await SendEmail(emailMessage);
            //var smtpClient = new SmtpClient
            //{
            //    Host = _configuration["EmailSettings:SmtpServer"],
            //    Port = Convert.ToInt32(_configuration["EmailSettings:SmtpPort"]),
            //    EnableSsl = true,
            //    Credentials = new NetworkCredential(_configuration["EmailSettings:SmtpUsername"], _configuration["EmailSettings:SmtpPassword"])
            //};

            //var mailMessage = new MailMessage
            //{
            //    From = new MailAddress(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:SenderName"]),
            //    Subject = emailMessage.Subject,
            //    Body = emailMessage.Body,
            //    IsBodyHtml = true
            //};

            //mailMessage.To.Add(emailMessage.ToEmails);

            //await smtpClient.SendMailAsync(mailMessage);
        }
        public async Task SendEmailConfirmationMessage(EmailMessage emailMessage)
        {
            emailMessage.Subject = "Hwllo Confirm Your email";
            emailMessage.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), emailMessage.PlaceHolders);
            await SendEmail(emailMessage);
        }
        private string GetEmailBody(string tempemailName)
        {
            var body = File.ReadAllText(string.Format(templatePath, tempemailName));
            return body;
        }
        private async Task SendEmail(EmailMessage emailMessage)
        {
            MailMessage mailMessage = new MailMessage
            {
                Subject = emailMessage.Subject,
                Body = emailMessage.Body,
                From = new MailAddress(_smtpconfig.SenderEmail, _smtpconfig.SenderName),
                IsBodyHtml = _smtpconfig.IsBodyHTML
            };
            //// for multiple email sending
            foreach (var toEmail in emailMessage.ToEmails)
            {
                mailMessage.To.Add(toEmail);
            }
            //mailMessage.To.Add(emailMessage.ToEmails);
            NetworkCredential networkCredential = new NetworkCredential(_smtpconfig.SmtpUsername, _smtpconfig.SmtpPassword);

            SmtpClient smtpClient = new SmtpClient
            {
                Host = _smtpconfig.SmtpServer,
                Port = _smtpconfig.SmtpPort,
                EnableSsl = _smtpconfig.EnableSSL,
                Credentials = networkCredential
            };
            mailMessage.BodyEncoding = Encoding.Default;
            await smtpClient.SendMailAsync(mailMessage);
        }

        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var PlaceHolder in keyValuePairs)
                {
                    if (text.Contains(PlaceHolder.Key))
                    {
                        text = text.Replace(PlaceHolder.Key, PlaceHolder.Value);
                    }
                }
            }
            return text;
        }
    }
}
