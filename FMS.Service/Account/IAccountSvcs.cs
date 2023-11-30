using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using Microsoft.AspNetCore.Identity;

namespace FMS.Service.Account
{
    public interface IAccountSvcs
    {
        Task<RegisterTokenModel> ChkToken(RegisterTokenModel data);
        Task<(IdentityResult result, AppUser user)> RegisterUser(SignUpUserModel data);
        Task<string> GenerateEmailConfirmationToken(AppUser data);
        Task<EmailConfirmModel> Mailconformed(string uid, string token);
        Task<EmailConfirmModel> SendMailToUser(string email);
        Task<bool> IsEmailInUse(string email);
        Task<(string Message, bool IsallowedToLogin)> PasswordSignIn(SignInModel data);
        Task SignOut();
        Task<ForgotPasswordModel> ForgotPassword(ForgotPasswordModel model);
        Task<Base> ResetPassword(ResetPasswordModel model);
        Task<Base> ChangePassword(ChangePasswordModel model);
    }
}
