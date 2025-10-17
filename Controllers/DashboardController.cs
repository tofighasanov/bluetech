using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Bluetech.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize] // ← пускаем только после входа
    public class DashboardController : Controller
    {
        public IActionResult Index() => View();
    }
}
