using FMS.Db.DbEntity;
using FMS.Model.CommonModel;
using FMS.Service.Account;
using FMS.Service.Devloper;
using FMS.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace FMS.Controllers.Account
{
    public class AccountController : Controller
    {
        #region Dependancy
        private readonly IAccountSvcs _accountSvcs;
        private readonly IDevloperSvcs _devloperSvcs;
        private readonly IWebHostEnvironment _hostingEnvironment;
        private readonly UserManager<AppUser> _userManager;
        private readonly IHttpContextAccessor _HttpContextAccessor;
        #endregion
        #region Constructor
        public AccountController(IAccountSvcs accountSvcs, IDevloperSvcs devloperSvcs, IWebHostEnvironment hostingEnvironment, UserManager<AppUser> userManager, IHttpContextAccessor httpContextAccessor)
        {
            _accountSvcs = accountSvcs;
            _hostingEnvironment = hostingEnvironment;
            _userManager = userManager;
            _devloperSvcs = devloperSvcs;
            _HttpContextAccessor = httpContextAccessor;
        }
        #endregion
        [AllowAnonymous, HttpGet]
        public IActionResult LandingPage()
        {
            return View();
        }
        #region SignUp

        [AllowAnonymous, HttpPost]
        public async Task<IActionResult> Token(RegisterTokenModel model)
        {
            var Obj = await _accountSvcs.ChkToken(model);
            return new JsonResult(Obj);
        }
        [HttpGet, AllowAnonymous]
        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost, AllowAnonymous]
        public async Task<IActionResult> SignUp(SignUpUserModel model)
        {
            /*-------------For Upload Profile pic in wwwroot images folder------------*/
            if (model.ProfilePhoto != null)
            {
                string StorageLocation = "images/ProfilePhoto/";
                string path = PictureStorage.UploadPhoto(model.ProfilePhoto, StorageLocation);
                string uploadsFolder = Path.Combine(_hostingEnvironment.WebRootPath, path);
                await model.ProfilePhoto.CopyToAsync(new FileStream(uploadsFolder, FileMode.Create));
                if (path != null)
                {
                    model.PhotoPath = path;
                }
            }
            var result = await _accountSvcs.RegisterUser(model);
            if (result.result.Succeeded)
            {
                //var IsSendMailSuccess = await _accountSvcs.GenerateEmailConfirmationToken(result.user);
                //return RedirectToAction("ConfirmEmail", "Account", new { eMail = model.Email });
                return RedirectToAction("Login", "Account", new { SuccessMsg = "Registration Successful" });
            }
            return View(model);
        }
        [AllowAnonymous, AcceptVerbs("Get")]
        public async Task<IActionResult> ConfirmEmail(string uid, string token, string email)
        {
            EmailConfirmModel model = new EmailConfirmModel
            {
                Email = email,
            };
            if (!string.IsNullOrEmpty(uid) && !string.IsNullOrEmpty(token))
            {
                var result = await _accountSvcs.Mailconformed(uid, token);
                if (result.EmailVerified)
                {
                    model.EmailVerified = result.EmailVerified;
                    return RedirectToAction("Login", "Account");
                }
            }
            return View(model);
        }
        [AllowAnonymous, AcceptVerbs("Post")]
        public async Task<IActionResult> ConfirmEmail(EmailConfirmModel model)
        {
            var result = await _accountSvcs.SendMailToUser(model.Email);
            if (result.EmailVerified)
            {
                model.EmailVerified = result.EmailVerified;
                //return RedirectToAction("Login", "Account", new {Message =result.SuccessMsg});
            }
            else
            {
                model.EmailSent = result.EmailSent;
            }
            return View(model);
        }
        [AllowAnonymous, AcceptVerbs("Get")]
        public async Task<IActionResult> IsEmailInUse(string email)
        {
            var result = await _accountSvcs.IsEmailInUse(email);
            return Json(result);
        }

        #endregion
        #region Login & LogOut
        [AllowAnonymous, AcceptVerbs("Get")]
        public  IActionResult Login(string ErrorMsg, string SuccessMsg)
        {
            if (ErrorMsg != null)
            {
                TempData["ErrorMsg"] = ErrorMsg;
            }
            if (SuccessMsg != null)
            {
                TempData["SuccessMsg"] = SuccessMsg;
            }
            return View();
        }
        [AllowAnonymous, AcceptVerbs("Post")]
        public async Task<IActionResult> Login(SignInModel signInModel, string returnUrl)
        {
            //remove the session Data
            _HttpContextAccessor.HttpContext.Session.Remove("BranchId");
            _HttpContextAccessor.HttpContext.Session.Remove("BranchName");
            _HttpContextAccessor.HttpContext.Session.Remove("FinancialYearId");
            _HttpContextAccessor.HttpContext.Session.Remove("FinancialYear");

            var result = await _accountSvcs.PasswordSignIn(signInModel);
            if (result.IsallowedToLogin)
            {
                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                return RedirectToAction("GoToPageAccordingToRole", "Account", new { SuccessMsg = result.Message + " " + signInModel.Email });
            }
            else
            {
                TempData["ErrorMsg"] = result.Message;
                return View(signInModel);
            }
        }

        [HttpGet, AllowAnonymous]
        public async Task<IActionResult> GoToPageAccordingToRole(string SuccessMsg)
        {
            //*******redirect user according to role******//
            // Get User
            if (User.Identity.IsAuthenticated)
            {
                var user = await _userManager.GetUserAsync(User);
                if (user != null)
                {
                    var roles = await _userManager.GetRolesAsync(user);
                    string userRole = roles.FirstOrDefault();
                    if (userRole == "Devloper")
                    {
                        return RedirectToAction("DashBoard", "DashBoard", new { SuccessMsg = SuccessMsg });
                    }
                    else if (userRole == "Admin")
                    {
                        return RedirectToAction("BranchAdmin", "DashBoard", new { SuccessMsg = SuccessMsg });
                    }
                    else if (userRole == "User")
                    {
                        return RedirectToAction("BranchUser", "DashBoard", new { SuccessMsg = SuccessMsg, UserId = user.Id });
                    }
                }
            }
            return View();
        }
        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _accountSvcs.SignOut();
            return RedirectToAction("LandingPage", "Account");
        }
        #endregion
        #region Forgot Password

        [AllowAnonymous, AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> ForgotPassword(ForgotPasswordModel model)
        {
            if (model != null)
            {
                var result = await _accountSvcs.ForgotPassword(model);
                if (result.ResponseCode == 200)
                {
                    model.EmailSent = result.EmailSent;
                }
                else
                {
                    model.EmailSent = false;
                }
            }

            return View(model);
        }

        [AllowAnonymous, AcceptVerbs("Get", "Post")]
        public async Task<IActionResult> ResetPassword(ResetPasswordModel model)
        {
            if (model != null)
            {
                var result = await _accountSvcs.ResetPassword(model);
                if (result.ResponseCode == 200)
                {
                    return RedirectToAction("Login", "Account", new { SuccessMsg = result.SuccessMsg.ToString() });
                }
                else
                {
                    TempData["ErrorMsg"] = result.ErrorMsg.ToString();
                }
            }
            return View(model);
        }
        #endregion
        #region Change Password

        [HttpPost, HttpGet]
        public async Task<IActionResult> ChangePassword(ChangePasswordModel model)
        {
            if (model != null)
            {
                var result = await _accountSvcs.ChangePassword(model);
                if (result.ResponseCode == 200)
                {

                    return RedirectToAction("Login", "Account", new { SuccessMsg = result.SuccessMsg.ToString() });
                }
                else
                {
                    TempData["Error"] = result.ErrorMsg.ToString();
                }
            }
            return View(model);
        }
        #endregion
    }
}

