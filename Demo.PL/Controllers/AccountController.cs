using Demo.DAL.Entities;
using Demo.PL.Helper;
using Demo.PL.Models.Account;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Demo.PL.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public AccountController(UserManager<ApplicationUser> userManager 
            , SignInManager<ApplicationUser> signInManager) 
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        #region SignUp

        public IActionResult SignUp()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignUp(RegisterViewModel registerViewModel)
        {
            if (ModelState.IsValid)
            {
                var user = new ApplicationUser()
                {
                    Email = registerViewModel.Email,
                    UserName = registerViewModel.Email.Split('@')[0],
                    IsAgree = registerViewModel.IsAgree,

                };
                var result = await _userManager.CreateAsync(user,registerViewModel.Password);
                if (result.Succeeded)
                     return RedirectToAction("SignIn"); 

                foreach (var error in result.Errors)
                    ModelState.AddModelError(string.Empty, error.Description);

            }
            return View(registerViewModel);
        }
        #endregion

        #region SignIn
        public IActionResult SignIn()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> SignIn(LoginViewModel loginViewModel)
        {
            if (ModelState.IsValid) 
            {
                var user = await _userManager.FindByEmailAsync(loginViewModel.Email);
                if(user is not null)
                {
                    var password = await _userManager.CheckPasswordAsync(user, loginViewModel.Password);
                    if(password)
                    {
                        var result = await _signInManager.PasswordSignInAsync(user, loginViewModel.Password, loginViewModel.RememberMe, false);
                       
                        if(result.Succeeded)
                          return RedirectToAction("Index", "Home");


                        ModelState.AddModelError(string.Empty, "Invalid Password");
                    }
                }
                ModelState.AddModelError(string.Empty, "Invalid Email");
            }
            return View(loginViewModel);
        }

        #endregion

        #region SignOut
        public async Task<IActionResult> SignOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("SignIn");
        }

        #endregion

        #region ForgetPassword

        public IActionResult ForgetPassword()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> ForgetPassword(ForgetPasswordViewModel forgetPasswordViewModel)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(forgetPasswordViewModel.Email);
                if(user is not null)
                {
                    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
                    var RsestPasswordLink = Url.Action("ResetPassword", "Account", new { Email = forgetPasswordViewModel.Email, Token = token }, Request.Scheme);
                    var email = new Email()
                    {
                        Title = "Reset Password",
                        Body = RsestPasswordLink,
                        To = forgetPasswordViewModel.Email
                    };
                    EmailSettings.SendEmail(email);

                    return RedirectToAction("CompleteForgetPassword");
                }
                ModelState.AddModelError(String.Empty, "Invalid Email");
            }
            return View(forgetPasswordViewModel);
        }

        #endregion
        public IActionResult CompleteForgetPassword()
        {
            return View();
        }
        #region Reset Password
        public IActionResult ResetPassword(string email , string token)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(ResetPasswordModelView resetPasswordModelView)
        {
            if(ModelState.IsValid)
            {
                var user = await _userManager.FindByEmailAsync(resetPasswordModelView.Email);
                if(user is not null )
                {
                    var result = await _userManager.ResetPasswordAsync(user, resetPasswordModelView.Token , resetPasswordModelView.Password);

                    if (result.Succeeded)
                        return RedirectToAction("ResetPasswordDone");

                    foreach (var error in result.Errors)
                        ModelState.AddModelError(String.Empty, error.Description);
                }
            }
            return View(resetPasswordModelView);
        }
        #endregion
        public IActionResult ResetPasswordDone()
        {
            return View();
        }

    }
}
