using FMS.Db.DbEntity;
using FMS.Model.CommonModel;

namespace FMS.Api.Email.EmailService
{
    public interface IEmailService
    {
        Task<bool> SendEmailConfirmationEmail(AppUser user, string token, UserEmailOptions options);
        Task<bool> SendForgotPasswordEmail(AppUser user, string token, UserEmailOptions options);
        Task SendExceptionEmail(string toEmail, string subject, string body);
    }
}
