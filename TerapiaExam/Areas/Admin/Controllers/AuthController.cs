using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TerapiaExam.Areas.Admin.ViewModels;
using TerapiaExam.Models;

namespace TerapiaExam.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AuthController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View();
            AppUser user = await _userManager.FindByEmailAsync(registerVM.Email);
            if(user is not null)
            {
                ModelState.AddModelError("Email", "This email is already used");
                return View();
            }
            user = await _userManager.FindByNameAsync(registerVM.UserName);
            if (user is not null)
            {
                ModelState.AddModelError("UserName", "This UserName is already used");
                return View();
            }

            user = new()
            {
                Name = registerVM.Name,
                Surname = registerVM.Surname,
                Email = registerVM.Email,
                UserName = registerVM.UserName,
            };
            var result = await _userManager.CreateAsync(user,registerVM.Password);
            if(!result.Succeeded) {
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(String.Empty, error.Description);
                }
                return View();
            }
            return RedirectToAction(nameof(Login));
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult>Login(LoginVM loginVM)
        {
            if(!ModelState.IsValid) return View();
            AppUser user =await _userManager.FindByEmailAsync(loginVM.UserNameOrEmail);
            if(user is null)
            {
                user = await _userManager.FindByNameAsync(loginVM.UserNameOrEmail);
                if(user is null)
                {
                    ModelState.AddModelError(String.Empty, "Username, email or password is incorrect");
                    return View();
                }
            }

            var result = await _signInManager.PasswordSignInAsync(user, loginVM.Password, false, false);
            if(!result.Succeeded)
            {
                ModelState.AddModelError(String.Empty, "Username, email or password is incorrect");
                return View();
            }

            return RedirectToAction("Index", "Home", new { area = "" });
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });
        }
    }
}
