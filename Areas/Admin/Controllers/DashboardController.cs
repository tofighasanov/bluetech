using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bluetech.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // доступ только после входа (Identity уже настроен)
    public class DashboardController : Controller
    {
        public IActionResult Index() => View();
    }
}
