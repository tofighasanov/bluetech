using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bluetech.Controllers
{
    public class AdminController : Controller
    {
        private readonly SignInManager<IdentityUser> _signIn;

        public AdminController(SignInManager<IdentityUser> signIn)
        {
            _signIn = signIn;
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(string email, string password)
        {
            var result = await _signIn.PasswordSignInAsync(email, password, isPersistent: true, lockoutOnFailure: false);
            if (result.Succeeded)
                return RedirectToAction("Index", "Dashboard", new { area = "Admin" });

            ViewBag.Error = "Неверный логин или пароль";
            return View();
        }

        public async Task<IActionResult> Logout()
        {
            await _signIn.SignOutAsync();
            return RedirectToAction("Login");
        }
    }
}
