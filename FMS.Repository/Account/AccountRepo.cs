using FMS.Db.Context;
using FMS.Db.DbEntity;
using FMS.Model;
using FMS.Model.CommonModel;
using FMS.Utility;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Security.Claims;
using System.Web;


namespace FMS.Repository.Account
{
    public class AccountRepo : IAccountRepo
    {
        #region Dependancy
        private readonly AppDbContext _appDbContext;
        private readonly ILogger<AccountRepo> _logger;
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;
        private readonly IHttpContextAccessor _httpContextAccessor;
        #endregion
        public AccountRepo(AppDbContext appDbContext, ILogger<AccountRepo> logger, UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, IHttpContextAccessor httpContextAccessor)
        {
            _appDbContext = appDbContext;
            _logger = logger;
            _userManager = userManager;
            _signInManager = signInManager;
            _httpContextAccessor = httpContextAccessor;
        }
        public async Task<Result<RegisterTokenModel>> IsTokenValid(RegisterTokenModel model)
        {
            Result<RegisterTokenModel> _Result = new ();
            try
            {
                _Result.IsSuccess = false;

                if (!string.IsNullOrEmpty(model.TokenValue))
                {
                    var _Query = await (from rt in _appDbContext.RegisterTokens
                                        join au in _appDbContext.AppUsers on rt.TokenId equals au.FkTokenId into auGroup
                                        from au in auGroup.DefaultIfEmpty()
                                        where rt.TokenValue == model.TokenValue && au == null
                                        select new RegisterTokenModel
                                        {
                                            TokenId = rt.TokenId,
                                            TokenValue = rt.TokenValue
                                        }).FirstOrDefaultAsync();

                    if (_Query != null)
                    {
                        _Result.SingleObjData = _Query;
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<(IdentityResult, AppUser)>> CreateUser(SignUpUserModel data)
        {
            Result<(IdentityResult result, AppUser user)> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                if (data != null)
                {
                    var user = new AppUser()
                    {
                        FkTokenId = data.FkTokenId,
                        Email = data.Email,
                        PhoneNumber = data.PhoneNumber,
                        UserName = data.Email,
                        Photo = data.PhotoPath,
                        Name = data.Name,
                    };
                    var identity = await _userManager.CreateAsync(user, data.Password);
                    if (identity.Succeeded)
                    {
                        _Result.SingleObjData = (identity, user);
                        _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Created);
                    }
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<string>> GenerateEmailConfirmationToken(AppUser data)
        {
            Result<string> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var User = new AppUser
                {
                    FkTokenId = data.FkTokenId,
                    Email = data.Email,
                    PhoneNumber = data.PhoneNumber,
                    UserName = data.UserName,

                    Photo = data.Photo,
                };
                var token = await _userManager.GenerateEmailConfirmationTokenAsync(User);
                if (!string.IsNullOrEmpty(token))
                {
                    _Result.SingleObjData = HttpUtility.UrlEncode(token);
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<IdentityResult>> Mailconformed(string uid, string token)
        {
            Result<IdentityResult> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
                {
                    _Result.SingleObjData = await _userManager.ConfirmEmailAsync(await _userManager.FindByIdAsync(uid), token);
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<(AppUser user, string token)>> MailNotconformed(string mail)
        {
            Result<(AppUser user, string token)> _Result = new();
            try
            {
                _Result.IsSuccess = false;

                if (!string.IsNullOrEmpty(mail))
                {
                    var user = await _userManager.FindByEmailAsync(mail);
                    if (user != null && !user.EmailConfirmed)
                    {
                        var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                        if (!string.IsNullOrEmpty(token))
                        {
                            _Result.SingleObjData = (user, token);
                        }
                    }
                    else
                    {
                        _Result.SingleObjData = (user, null);
                    }
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> IsEmailInUse(string email)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;

                var user = await _userManager.FindByEmailAsync(email);

                if (user != null)
                {
                    _Result.SingleObjData = true;
                }
                else
                {
                    _Result.SingleObjData = false;
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<SignInResult>> PasswordSignIn(SignInModel model)
        {
            Result<SignInResult> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, true);
          
                _Result.SingleObjData = result;
                _Result.Response = ResponseStatusExtensions.ToStatusString(ResponseStatus.Status.Success);
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task SignOut()
        {
            await _signInManager.SignOutAsync();
        }
        public async Task<Result<(AppUser user, string token)>> ForgotPassword(ForgotPasswordModel model)
        {
            Result<(AppUser user, string token)> _Result = new();

            try
            {
                _Result.IsSuccess = false;
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user != null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    _Result.SingleObjData = (user, token);
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> ResetPassword(ResetPasswordModel model)
        {
            Result<bool> _Result = new();

            try
            {
                _Result.IsSuccess = false;

                model.Token = model.Token.Replace(' ', '+');

                var result = await _userManager.ResetPasswordAsync(await _userManager.FindByIdAsync(model.UserId), model.Token, model.NewPassword); ;

                if (result.Succeeded)
                {
                    _Result.SingleObjData = result.Succeeded;
                }
                _Result.IsSuccess = true;
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
        public async Task<Result<bool>> ChangePassword(ChangePasswordModel model)
        {
            Result<bool> _Result = new();
            try
            {
                _Result.IsSuccess = false;
                var userId = _httpContextAccessor.HttpContext.User?.FindFirstValue(ClaimTypes.NameIdentifier); ;
                var user = await _userManager.FindByIdAsync(userId);

                if (user != null)
                {
                    var result = await _userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);

                    if (result.Succeeded)
                    {
                        _Result.SingleObjData = result.Succeeded;
                    }
                    //    _Result.Message = "Password Updated Successfully!";
                    //}
                    //else
                    //{
                    //    foreach (var err in result.Errors)
                    //    {
                    //        _Result.Message = err.Description;

                    //    }
                    //}
                    _Result.IsSuccess = true;
                }
            }
            catch (Exception _Exception)
            {
                _Result.Exception = _Exception;
            }
            return _Result;
        }
    }
}
