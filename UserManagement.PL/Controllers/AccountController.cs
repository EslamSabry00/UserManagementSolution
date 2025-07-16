using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using UserManagement.PL.ViewModels;
using UserManagement.DAL.Models;
using System.Threading.Tasks;
using DataAccessLayer.Entities;
using UserManagement.PL.Helpers;



namespace UserManagement.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly IEmailSettings _emailSettings;
        private readonly ISmsService _smsService;

        public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager,IEmailSettings emailSettings, ISmsService smsService) {
            _userManager = userManager;
            _signInManager = signInManager;
            _emailSettings = emailSettings;
            _smsService = smsService;
        }
        #region Register

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (ModelState.IsValid) {
                var user = new ApplicationUser()
                {
                    UserName = model.Email.Split('@')[0],
                    Email = model.Email,
                    IsAgree = model.IsAgree,
                    Name = model.Name,
                };
                var result = await _userManager.CreateAsync(user, model.Password);
                if (result.Succeeded) {
                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }
        #endregion

        #region Login

        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid) { 
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null) {
                    var flag = await _userManager.CheckPasswordAsync(user, model.Password);
                    if (flag) {
                        var result = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe,false);
                        if (result.Succeeded) {
                            return RedirectToAction("Index", "Home");
                        }
                    }
                    ModelState.AddModelError(string.Empty, "Password incorrect");
                }
                ModelState.AddModelError(string.Empty, "Email is not existed");
            }
            return View(model);
        }

        #endregion

        #region Sign out

        public new async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Login));
        }

        #endregion

        #region ForgetPassword
       public IActionResult ForgetPassword()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendEmail(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid) {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null) {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = user.Email, token = token  },Request.Scheme);
                    var email = new Email()
                     {
                        Subject = "Reset Password",
                        To = model.Email,
                        Body = ResetPasswordLink
                    };
                    _emailSettings.SendEmail(email);
                    return RedirectToAction(nameof(CheckInpox));
                }
                ModelState.AddModelError(string.Empty, "Email not Exist");
            }
            return View(model);

        }

        [HttpPost]
        public async Task<IActionResult> SendSms(ForgetPasswordViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(model.Email);
                if (user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var ResetPasswordLink = Url.Action("ResetPassword", "Account", new { email = user.Email, token = token }, Request.Scheme);
                    var sms = new SmsMessage()
                    {
                        PhoneNumber = user.PhoneNumber,
                        Body = ResetPasswordLink
                    };
                    _smsService.SendSms(sms);
                    return Ok("Check Phone SMS");
                }
                ModelState.AddModelError(string.Empty, "Email not Exist");
            }
            return View(model);

        }

        public IActionResult CheckInpox()
        {
            return View();
        }
        #endregion

        #region reset password
        public IActionResult ResetPassword(string email, string token)
        {
            TempData["email"] = email;
            TempData["token"] = token;
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordViewModel model)
        {
            if(ModelState.IsValid) {
                string email = TempData["email"] as string;
                string token = TempData["token"] as string;
                var user = await _userManager.FindByEmailAsync(email);
                var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
                if (result.Succeeded) {
                    return RedirectToAction(nameof(Login));
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }
            return View(model);
        }

        #endregion
    }
}
