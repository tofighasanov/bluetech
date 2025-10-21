using System;
using Bluetech.Models;
using Microsoft.AspNetCore.Mvc;

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
            if (!string.Equals(model.Check?.Trim(), "7", StringComparison.Ordinal)) //   "3+4"
            {
                ModelState.AddModelError(nameof(model.Check), " .");
            }

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            TempData["ContactOk"] = "!      email.";

            return RedirectToAction(nameof(Contact));
        }
    }
}