using FMS.Api.Email.EmailService;
using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Repository.Account;
using FMS.Utility;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;

namespace FMS.Service.Account
{
    public class AccountSvcs : IAccountSvcs
    {
        private readonly IAccountRepo _accountRepo;
        private readonly IEmailService _emailService;
        private readonly IConfiguration _configuration;
        public AccountSvcs(IAccountRepo accountRepository, IEmailService emailService, IConfiguration configuration)
        {
            _accountRepo = accountRepository;
            _emailService = emailService;
            _configuration = configuration;
        }
        public async Task<RegisterTokenModel> ChkToken(RegisterTokenModel data)
        {
            RegisterTokenModel Obj = new();
            var Result = await _accountRepo.IsTokenValid(data);
            try
            {
                if (Result.IsSuccess)
                {
                    if (Result.Response == "success")
                    {
                        Obj = new RegisterTokenModel()
                        {
                            TokenId = Result.SingleObjData.TokenId,
                            TokenValue = Result.SingleObjData.TokenValue,
                            ResponseStatus = "found",
                            ResponseCode = Convert.ToInt32(ResponseCode.Status.Found),
                            SuccessMsg = "Thanks For Verify Your Register Token"
                        };
                    }
                    else
                    {
                        Obj = new RegisterTokenModel()
                        {
                            ResponseStatus = "notfound",
                            ResponseCode = Convert.ToInt32(ResponseCode.Status.NotFound),
                            ErrorMsg = "Register Token: " + data.TokenValue + " Is Invalid"
                        };
                    }

                }
            }
            catch
            {
                Obj = new RegisterTokenModel()
                {
                    Exception = Result.SingleObjData.Exception,
                    ResponseStatus = "error",
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    ErrorMsg = "Some Error Occourd Try To Contact Your App Devloper"
                };
            }
            return Obj;
        }
        public async Task<(IdentityResult, AppUser)> RegisterUser(SignUpUserModel model)
        {
            var Result = await _accountRepo.CreateUser(model);
            return Result.SingleObjData; ;
        }
        public async Task<string> GenerateEmailConfirmationToken(AppUser user)
        {
            var result = await _accountRepo.GenerateEmailConfirmationToken(user);
            if (result.IsSuccess)
            {
                //*****************************************************************************************//
                string appDomain = _configuration.GetSection("Application:AppDomain").Value;
                string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;
                UserEmailOptions options = new()
                {
                    ToEmails = new List<string>() { user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", user.UserName),
                    new KeyValuePair<string, string>("{{Link}}",
                        string.Format(appDomain + confirmationLink, user.Id, result.SingleObjData))
                }
                };
                //***************************************************************************************//
                await _emailService.SendEmailConfirmationEmail(user, result.SingleObjData, options);
            }
            return result.SingleObjData;
        }
        public async Task<EmailConfirmModel> Mailconformed(string uid, string token)
        {
            EmailConfirmModel model = new();
            //string Replacedtoken = token.Replace(' ', '+');
            var IsMailConformed = await _accountRepo.Mailconformed(uid, token.Replace(' ', '+'));
            if (IsMailConformed.IsSuccess && IsMailConformed.SingleObjData.Succeeded)
            {
                model.EmailVerified = true;
            }
            return model;
        }
        public async Task<EmailConfirmModel> SendMailToUser(string mail)
        {
            EmailConfirmModel model = new();
            var IsMailConformed = await _accountRepo.MailNotconformed(mail);

            if (IsMailConformed.SingleObjData.token != null && IsMailConformed.SingleObjData.user != null)
            {
                //*****************************************************************************************//
                string appDomain = _configuration.GetSection("Application:AppDomain").Value;
                string confirmationLink = _configuration.GetSection("Application:EmailConfirmation").Value;
                UserEmailOptions options = new()
                {
                    ToEmails = new List<string>() { IsMailConformed.SingleObjData.user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", IsMailConformed.SingleObjData.user.UserName),
                    new KeyValuePair<string, string>("{{Link}}",
                    string.Format(appDomain + confirmationLink, IsMailConformed.SingleObjData.user.Id, IsMailConformed.SingleObjData.token))
                }
                };
                //***************************************************************************************//
                bool result = await _emailService.SendEmailConfirmationEmail(IsMailConformed.SingleObjData.user, IsMailConformed.SingleObjData.token, options);
                if (result)
                {
                    model.EmailSent = result;
                    model.SuccessMsg = "We Send Conformation Mail To your Account Plz Conform It";
                }
                else
                {
                    model.EmailSent = result;
                    model.ErrorMsg = "Failed to Send Conformation Mail";
                }
            }
            if (IsMailConformed.SingleObjData.user.EmailConfirmed)
            {
                model.EmailVerified = IsMailConformed.SingleObjData.user.EmailConfirmed;
                model.SuccessMsg = "Registration Successfull";
            }
            return model;
        }
        public async Task<bool> IsEmailInUse(string email)
        {
            var result = await _accountRepo.IsEmailInUse(email);
            return result.SingleObjData;
        }
        public async Task<(string, bool)> PasswordSignIn(SignInModel data)
        {
            (string Message, bool IsallowedToLogin) User;
            var result = await _accountRepo.PasswordSignIn(data);
            if (result.Exception == null)
            {
                if (result.SingleObjData.Succeeded)
                {
                    User.Message = "Welcome";
                    User.IsallowedToLogin = true;
                }
                else if (result.SingleObjData.IsNotAllowed)
                {
                    User.Message = "You already Have Account But You Not Conform Your email, Plz check Your Email we send a Link to Conform Your acoount when you register in our site";
                    User.IsallowedToLogin = false;
                }
                else if (result.SingleObjData.IsLockedOut)
                {
                    User.Message = "Accout Lock Due To You Reach Maximum Login Attempt Try After 5 minutes";
                    User.IsallowedToLogin = false;
                }
                else
                {
                    User.Message = "Invalid Credential";
                    User.IsallowedToLogin = false;
                }
            }
            else
            {
                User.Message = result.Exception.InnerException.Message;
                User.IsallowedToLogin = false;
            }
            return User;
        }
        public async Task SignOut()
        {
            await _accountRepo.SignOut();
        }
        public async Task<ForgotPasswordModel> ForgotPassword(ForgotPasswordModel model)
        {
            ForgotPasswordModel Obj = new();
            var result = await _accountRepo.ForgotPassword(model);
            if (result.SingleObjData.user != null && result.SingleObjData.token != null)
            {
                string appDomain = _configuration.GetSection("Application:AppDomain").Value;
                string confirmationLink = _configuration.GetSection("Application:ForgotPassword").Value;

                UserEmailOptions options = new()
                {
                    ToEmails = new List<string>() { result.SingleObjData.user.Email },
                    PlaceHolders = new List<KeyValuePair<string, string>>()
                {
                    new KeyValuePair<string, string>("{{UserName}}", result.SingleObjData.user.Name),
                    new KeyValuePair<string, string>("{{Link}}", string.Format(appDomain + confirmationLink, result.SingleObjData.user.Id, result.SingleObjData.token))
                }
                };
                var IsMailSend = await _emailService.SendForgotPasswordEmail(result.SingleObjData.user, result.SingleObjData.token, options);
                if (IsMailSend)
                {
                    Obj = new ForgotPasswordModel
                    {
                        ResponseStatus = ResponseStatus.Status.Success.ToString(),
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                        EmailSent = IsMailSend,
                        SuccessMsg = "We Send Conformation Mail To your Account Plz Conform It"
                    };
                }
                else
                {
                    Obj = new ForgotPasswordModel
                    {
                        ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                        EmailSent = IsMailSend,
                        ErrorMsg = "Failed to Send Conformation Mail"
                    };
                }
            }
            return Obj;
        }
        public async Task<Base> ResetPassword(ResetPasswordModel model)
        {
            var result = await _accountRepo.ResetPassword(model);
            Base Obj;
            if (result.SingleObjData)
            {
                Obj = new()
                {
                    ResponseStatus = ResponseStatus.Status.Success.ToString(),
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                    SuccessMsg = "Password Updated Successfully"
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = ResponseStatus.Status.Success.ToString(),
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    ErrorMsg = "Password Updated Failed"
                };
            }
            return Obj;
        }
        public async Task<Base> ChangePassword(ChangePasswordModel model)
        {
            var result = await _accountRepo.ChangePassword(model);
            Base Obj;
            if (result.SingleObjData)
            {
                Obj = new()
                {
                    ResponseStatus = ResponseStatus.Status.Success.ToString(),
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.OK),
                    SuccessMsg = "Password Changed Successfully"
                };
            }
            else
            {
                Obj = new()
                {
                    ResponseStatus = ResponseStatus.Status.Success.ToString(),
                    ResponseCode = Convert.ToInt32(ResponseCode.Status.BadRequest),
                    ErrorMsg = "Password Change Failed"
                };
            }
            return Obj;
        }
    }
}
