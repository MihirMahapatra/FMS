
using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using Microsoft.Extensions.Options;
using System.Net;
using System.Net.Mail;

namespace FMS.Api.Email.EmailService
{
    public class EmailService : IEmailService
    {

        private const string templatePath = @"EmailTemplate/{0}.html";
        private readonly SMTPConfigModel _smtpConfig;
        public EmailService(IOptions<SMTPConfigModel> smtpConfig)
        {
            _smtpConfig = smtpConfig.Value;
        }

        public async Task<bool> SendEmailConfirmationEmail(AppUser user, string token, UserEmailOptions options)
        {
            try
            {
                options.Subject = UpdatePlaceHolders("Hello {{UserName}}, Confirm your email id.", options.PlaceHolders);
                options.Body = UpdatePlaceHolders(GetEmailBody("EmailConfirm"), options.PlaceHolders);
                await SendEmail(options);
                return true;
            }
            catch
            {
                return false;
            }

        }
        private string UpdatePlaceHolders(string text, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(text) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (text.Contains(placeholder.Key))
                    {
                        text = text.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return text;
        }
        private string GetEmailBody(string templateName)
        {
            var body = File.ReadAllText(string.Format(templatePath, templateName));
            return body;
        }
        private async Task SendEmail(UserEmailOptions userEmailOptions)
        {
            MailMessage mail = new()
            {
                Subject = userEmailOptions.Subject,
                Body = userEmailOptions.Body,
                From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName),
                IsBodyHtml = _smtpConfig.IsBodyHTML,
            };

            foreach (var toEmail in userEmailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }

            SmtpClient smtpClient = new SmtpClient(_smtpConfig.Host)
            {
                Port = _smtpConfig.Port,
                EnableSsl = _smtpConfig.EnableSSL,
                Credentials = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password)
            };
            await smtpClient.SendMailAsync(mail);
        }
        public async Task<bool> SendForgotPasswordEmail(AppUser user, string token, UserEmailOptions options)
        {
            try
            {
                await SendEmailForForgotPassword(options);
                return true;
            }
            catch
            {
                return false;
            }
        }
        private async Task SendEmailForForgotPassword(UserEmailOptions userEmailOptions)
        {
            userEmailOptions.Subject = UpdatePlaceHolders("Hello {{UserName}}, reset your password.", userEmailOptions.PlaceHolders);

            userEmailOptions.Body = UpdatePlaceHolders(GetEmailBody("ForgotPassword"), userEmailOptions.PlaceHolders);

            await SendEmail(userEmailOptions);
        }

        public async Task SendExceptionEmail(string toEmail, string subject, string body)
        {
            try
            {
                var smtpClient = new SmtpClient(_smtpConfig.Host)
                {
                    Port = _smtpConfig.Port,
                    Credentials = new NetworkCredential(_smtpConfig.UserName, _smtpConfig.Password),
                    EnableSsl = _smtpConfig.EnableSSL,
                };
                using (var message = new MailMessage())
                {
                    message.From = new MailAddress(_smtpConfig.SenderAddress, _smtpConfig.SenderDisplayName);
                    message.To.Add(toEmail);
                    message.Subject = subject;
                    message.Body = body;
                    message.IsBodyHtml = _smtpConfig.IsBodyHTML;
                    await smtpClient.SendMailAsync(message);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
            }
               
        }
    }
}
