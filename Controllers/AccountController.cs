using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SkillsTest.Models;

namespace SkillsTest.Controllers
{
    /// <summary>
    /// Контроллер аутентификации/аторизации.
    /// </summary>
    public class AccountController : Controller
    {
        private SignInManager<IdentityUser> _signInManager;
        private UserManager<IdentityUser> _userManager;

        public AccountController(SignInManager<IdentityUser> signInManager)
        {
            _signInManager = signInManager ?? throw new ArgumentNullException(nameof(signInManager));
            _userManager = _signInManager.UserManager;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterBindingModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterBindingModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                ModelState.AddModelError("Email", "Пользователь с таким e-mail уже существует.");
                return View(model);
            }

            user = new IdentityUser
            {
                UserName = model.Email,
                Email = model.Email,
                EmailConfirmed = true
            };
            var createResult = await _userManager.CreateAsync(user, model.Password);
            if (!createResult.Succeeded)
            {
                ModelState.AddModelError("", "Не удалось зарегистрировать пользователя.");
                // Принадлежность некоторых ошибок Identity можно идентифицировать по первому слову текстового
                // свойства 'Code'.
                foreach (var error in createResult.Errors)
                    ModelState.AddModelError(error.Code.StartsWith("Password") ? "Password" : "", error.Description);
                return View(model);
            }

            // Выйти из приложения (удалить куки авторизации).
            await _signInManager.SignOutAsync();

            // Перенаправить пользователя на страницу логина.
            return RedirectToAction(nameof(Login));
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginBindingModel());
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginBindingModel model, string returnUrl = null)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user == null)
            {
                ModelState.AddModelError("Email", "Пользователь с таким e-mail не найден.");
                return View(model);
            }

            if (!await _userManager.CheckPasswordAsync(user, model.Password))
            {
                ModelState.AddModelError("Password", "Неверный пароль.");
                return View(model);
            }

            // На всякий случай выйти из приложения (удалить куки авторизации).
            await _signInManager.SignOutAsync();

            // Пробуем войти в приложении. В случае удачи в ответ сервера будут включены куки авторизации.
            var signInResult = await _signInManager.PasswordSignInAsync(user, model.Password, model.RememberMe, false);
            if (!signInResult.Succeeded)
            {
                ModelState.AddModelError("", "Не удалось войти на сайт.");
                return View(model);
            }

            // Если в строке запроса имеется 'returnUrl' (локальный) - переходим по нему. Иначе переходим на
            // главную страницу.
            string redirectUrl = string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl)
                 ? returnUrl
                 : Url.Action("Index", "Movies");
            return LocalRedirect(redirectUrl);
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Movies");
        }
    }
}
