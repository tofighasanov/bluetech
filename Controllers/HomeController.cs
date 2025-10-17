using System;
using Microsoft.AspNetCore.Mvc;
using Bluetech.Models;

namespace Bluetech.Controllers
{
    public class HomeController : Controller
    {
        private const string V = "�������! �� �������� � ���� �� email.";

        public IActionResult Index() => View();
        public IActionResult About() => View();
        public IActionResult Contact() => View(new ContactFormModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactFormModel model)
        {
            var answer = model.Check?.Trim();
            if (!string.Equals(answer, "7", StringComparison.Ordinal)) // ����������� �������� "3+4"
            {
                ModelState.AddModelError(nameof(model.Check), "����� �� ����������� ������ ��������.");
            }

            if (!ModelState.IsValid)
                return View(model);

            // TODO: �������� ������. ����� ������ ��������.
            TempData["ContactOk"] = "�������! �� �������� � ���� �� email.";

            return RedirectToAction(nameof(Contact));
        }
    }
}
