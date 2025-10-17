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
            if (model.Check.Trim() != "7") // ����������� �������� "3+4"
                ModelState.AddModelError(nameof(model.Check), "����� ��������.");

            if (!ModelState.IsValid)
                return View(model);

            // TODO: �������� ������. ����� ������ ��������.
            TempData["ContactOk"] = "�������! �� �������� � ���� �� email.";

            return RedirectToAction(nameof(Contact));
        }
    }
}
