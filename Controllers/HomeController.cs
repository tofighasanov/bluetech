using System;
using System.Security.Cryptography;
using Bluetech.Models;
using Bluetech.Services;
using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace Bluetech.Controllers
{
    public class HomeController : Controller
    {
        private readonly IArithmeticCaptchaService _captchaService;
        private readonly IDataProtector _captchaProtector;

        public HomeController(
            IArithmeticCaptchaService captchaService,
            IDataProtectionProvider dataProtectionProvider)
        {
            _captchaService = captchaService;
            _captchaProtector = dataProtectionProvider.CreateProtector("contact-form-captcha");
        }

        public IActionResult Index() => View();

        public IActionResult About() => View();

        public IActionResult Contact() => View(CreateContactFormModel());

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Contact(ContactFormModel model)
        {
            ValidateCaptcha(model);

            if (!ModelState.IsValid)
            {
                RefreshCaptcha(model);
                return View(model);
            }

            TempData["ContactOk"] = "Спасибо! Мы свяжемся с вами по указанному email.";

            return RedirectToAction(nameof(Contact));
        }

        private void ValidateCaptcha(ContactFormModel model)
        {
            if (string.IsNullOrWhiteSpace(model.CaptchaToken))
            {
                ModelState.AddModelError(nameof(model.Check), "Проверочный вопрос истёк, попробуйте ещё раз.");
                return;
            }

            try
            {
                var expectedAnswer = _captchaProtector.Unprotect(model.CaptchaToken);
                if (!string.Equals(expectedAnswer, model.Check?.Trim(), StringComparison.Ordinal))
                {
                    ModelState.AddModelError(nameof(model.Check), "Ответ на проверочный вопрос неверный.");
                }
            }
            catch (CryptographicException)
            {
                ModelState.AddModelError(nameof(model.Check), "Проверочный вопрос истёк, попробуйте ещё раз.");
            }
        }

        private void RefreshCaptcha(ContactFormModel model)
        {
            var challenge = _captchaService.GenerateChallenge();
            model.CaptchaQuestion = challenge.Question;
            model.CaptchaToken = _captchaProtector.Protect(challenge.Answer);
            model.Check = string.Empty;

            ModelState.SetModelValue(nameof(ContactFormModel.CaptchaQuestion), model.CaptchaQuestion, model.CaptchaQuestion);
            ModelState.SetModelValue(nameof(ContactFormModel.CaptchaToken), model.CaptchaToken, model.CaptchaToken);
            ModelState.SetModelValue(nameof(ContactFormModel.Check), string.Empty, string.Empty);
        }

        private ContactFormModel CreateContactFormModel()
        {
            var challenge = _captchaService.GenerateChallenge();

            return new ContactFormModel
            {
                CaptchaQuestion = challenge.Question,
                CaptchaToken = _captchaProtector.Protect(challenge.Answer)
            };
        }
    }
}