using System.Threading.Tasks;
using BLL.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TestMVC.Extensions;
using TestMVC.Models;
using TestMVC.ViewModels;

namespace CustomIdentityApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> userManager;
        private readonly SignInManager<User> signInManager;
        private readonly IOrderService orderService;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, IOrderService orderService)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.orderService = orderService;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return this.View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                User user = new User { Email = model.Email, UserName = model.Email, Name = model.Name, SurName = model.SurName };
                // add user
                var result = await this.userManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    // confirm email service
                    // генерация токена для пользователя
                    // var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);
                    // var callbackUrl = Url.Action(
                    //    "ConfirmEmail",
                    //    "Account",
                    //    new { userId = user.Id, code = code },
                    //    protocol: HttpContext.Request.Scheme);
                    // EmailService emailService = new EmailService();
                    // await emailService.SendEmailAsync(model.Email, "Confirm your account",
                    // $"Подтвердите регистрацию, перейдя по ссылке: <a href='{callbackUrl}'>link</a>");
                    // return Content("Для завершения регистрации проверьте электронную почту и перейдите по ссылке, указанной в письме");

                    this.MigrateBasketFromCookieToDB(user.Id);

                    // установка куки
                    await this.signInManager.SignInAsync(user, false);
                    return this.RedirectToAction("Index", "Home");
                }
                else
                {
                    foreach (var error in result.Errors)
                    {
                        this.ModelState.AddModelError(string.Empty, error.Description);
                    }
                }
            }

            return this.View(model);
        }

        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> ConfirmEmail(string userId, string code)
        {
            if (userId == null || code == null)
            {
                return this.View("Error");
            }

            var user = await this.userManager.FindByIdAsync(userId);
            if (user == null)
            {
                return this.View("Error");
            }

            var result = await this.userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return this.RedirectToAction("Index", "Home");
            }
            else
            {
                return this.View("Error");
            }
        }

        // Login
        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            return this.View(new LoginViewModel { ReturnUrl = returnUrl });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (this.ModelState.IsValid)
            {
                var user = await this.userManager.FindByNameAsync(model.Email);
                var result =
                    await this.signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    this.MigrateBasketFromCookieToDB(user.Id);

                    // check confirmed email
                    // if (!user.EmailConfirmed)
                    // {
                    //    await _signInManager.SignOutAsync();
                    //    ModelState.AddModelError("", "Email not confirmed");
                    //    return View(model);
                    // }

                    // проверяем, принадлежит ли URL приложению
                    if (!string.IsNullOrEmpty(model.ReturnUrl) && this.Url.IsLocalUrl(model.ReturnUrl))
                    {
                        return this.Redirect(model.ReturnUrl);
                    }
                    else
                    {
                        return this.RedirectToAction("Index", "Home");
                    }
                }
                else
                {
                    this.ModelState.AddModelError(string.Empty, "Неправильный логин и (или) пароль");
                }
            }

            return this.View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            // удаляем аутентификационные куки
            await this.signInManager.SignOutAsync();
            return this.RedirectToAction("Index", "Home");
        }

        private void MigrateBasketFromCookieToDB(string userId)
        {
            if (this.HttpContext.IsContainBasket())
            {
                this.orderService.MigrateBasketFromCookie(userId, this.HttpContext.GetBasket());
                this.HttpContext.RemoveBasketFromSession();
            }
        }
    }
}