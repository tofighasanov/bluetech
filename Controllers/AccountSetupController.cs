using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Bluetech.Controllers
{
    public class AccountSetupController : Controller
    {
        private readonly UserManager<IdentityUser> _users;

        public AccountSetupController(UserManager<IdentityUser> users)
        {
            _users = users;
        }

        // ВРЕМЕННО: сходи 1 раз по этому URL, затем удаляй контроллер
        public async Task<IActionResult> CreateAdmin()
        {
            var email = "admin@bluetech.az";
            var exists = await _users.FindByEmailAsync(email);
            if (exists != null) return Content("Admin already exists");

            var user = new IdentityUser { UserName = email, Email = email, EmailConfirmed = true };
            var result = await _users.CreateAsync(user, "Admin123!");
            return Content(result.Succeeded ? "Admin created" : string.Join("; ", result.Errors.Select(e => e.Description)));
        }
    }
}
