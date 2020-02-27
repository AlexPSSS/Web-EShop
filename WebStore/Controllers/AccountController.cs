using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebStore.Domain.Entities;
using WebStore.Domain.Entities.Identity;
using WebStore.Domain.Models;

namespace WebStore.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        private readonly ILogger<AccountController> _Logger;

        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager, ILogger<AccountController> Logger)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _Logger = Logger;
        }

        // GET: /<controller>/
        [HttpGet]
        public IActionResult Login()
        {
            return View(new LoginViewModel());
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var loginResult = await _signInManager.PasswordSignInAsync(model.UserName,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false); //Проверяем логин/пароль пользователя

            if (!loginResult.Succeeded)
            {
                ModelState.AddModelError("", "Вход невозможен");//говорим пользователю что вход невозможен
                return View(model);
            }

            if (Url.IsLocalUrl(model.ReturnUrl)) //если returnUrl - локальный
            {
                return Redirect(model.ReturnUrl); //перенаправляем туда, откуда пришли
            }

            return RedirectToAction("Index", "Home"); //иначе на главную
        }

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterUserViewModel());
        }

        //[HttpPost, ValidateAntiForgeryToken]
        //public async Task<IActionResult> Register(RegisterUserViewModel model)
        //{
        //    if (!ModelState.IsValid)
        //        return View(model);

        //    var user = new User { UserName = model.UserName, Email = model.Email };
        //    var createResult = await _userManager.CreateAsync(user, model.Password);

        //    if (!createResult.Succeeded)
        //    {
        //        foreach (var identityError in createResult.Errors) //выводим ошибки
        //        {
        //            ModelState.AddModelError("", identityError.Description);
        //            return View(model);
        //        }
        //    }

        //    await _signInManager.SignInAsync(user, false);
        //    await _userManager.AddToRoleAsync(user, "User");
        //    return RedirectToAction("Index", "Home");
        //}

        [HttpPost, ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUserViewModel Model, [FromServices] IMapper Mapper)
        {
            if (!ModelState.IsValid)
                return View(Model);

            //var user = new User
            //{
            //    UserName = Model.UserName
            //};
            var user = Mapper.Map<User>(Model);

            _Logger.LogInformation("Регистрация нового пользователя {0}", Model.UserName);

            var registration_result = await _userManager.CreateAsync(user, Model.Password);
            if (registration_result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, Role.User);
                _Logger.LogInformation("Пользователь {0} успешно зарегистрирован", Model.UserName);
                await _signInManager.SignInAsync(user, false);
                _Logger.LogInformation("Пользователь {0} вошёл в систему", Model.UserName);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in registration_result.Errors)
                ModelState.AddModelError("", error.Description);

            _Logger.LogWarning("Ошибка при регистрации нового пользователя {0}:{1}",
                Model.UserName, string.Join(", ", registration_result.Errors.Select(e => e.Description)));

            return View(Model);
        }

    }
}