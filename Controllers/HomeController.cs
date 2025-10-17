using Microsoft.AspNetCore.Mvc;
using Bluetech.Models;

namespace Bluetech.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View(new ContactFormModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactFormModel model)
        {
            if (model.Check.Trim() != "7") // примитивная проверка "3+4"
                ModelState.AddModelError(nameof(model.Check), "Ответ неверный.");

            if (!ModelState.IsValid)
                return View(model);

            // TODO: отправка письма. Здесь просто имитация.
            TempData["ContactOk"] = "Спасибо! Мы свяжемся с вами по email.";

            return RedirectToAction(nameof(Contact));
        }
    }
}
