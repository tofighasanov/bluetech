using System;
using Microsoft.AspNetCore.Mvc;
using Bluetech.Models;

namespace Bluetech.Controllers
{
    public class HomeController : Controller
    {
        private const string V = "Спасибо! Мы свяжемся с вами по email.";

        public IActionResult Index() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View(new ContactFormModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactFormModel model)
        {
            var answer = model.Check?.Trim();
            if (!string.Equals(answer, "7", StringComparison.Ordinal)) // примитивная проверка "3+4"
            {
                ModelState.AddModelError(nameof(model.Check), "Ответ на проверочный вопрос неверный.");
            }

            if (!ModelState.IsValid)
                return View(model);

            // TODO: отправка письма. Здесь просто имитация.
            TempData["ContactOk"] = "Спасибо! Мы свяжемся с вами по email.";

            return RedirectToAction(nameof(Contact));
        }
    }
}
