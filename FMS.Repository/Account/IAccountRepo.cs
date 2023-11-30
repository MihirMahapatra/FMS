using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using Microsoft.AspNetCore.Identity;

namespace FMS.Repository.Account

{
    public interface IAccountRepo
    {
        Task<Result<RegisterTokenModel>> IsTokenValid(RegisterTokenModel data);
        Task<Result<(IdentityResult result, AppUser user)>> CreateUser(SignUpUserModel data);
        Task<Result<string>> GenerateEmailConfirmationToken(AppUser data);
        Task<Result<IdentityResult>> Mailconformed(string uid, string token);
        Task<Result<(AppUser user, string token)>> MailNotconformed(string mail);
        Task<Result<bool>> IsEmailInUse(string email);
        Task<Result<SignInResult>> PasswordSignIn(SignInModel model);
        Task SignOut();
        Task<Result<(AppUser user, string token)>> ForgotPassword(ForgotPasswordModel model);
        Task<Result<bool>> ResetPassword(ResetPasswordModel model);
        Task<Result<bool>> ChangePassword(ChangePasswordModel model);
    }
}
