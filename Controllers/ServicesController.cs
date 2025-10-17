using Microsoft.AspNetCore.Mvc;
using Bluetech.Models;
using System.Collections.Generic;

namespace Bluetech.Controllers
{
    public class ServicesController : Controller
    {
        public IActionResult Index()
        {
            var services = new List<Service>
            {
                new Service { Name = "Информационные системы", Description = "Разработка, интеграция и сопровождение решений под ключ." },
                new Service { Name = "Автоматизация бизнеса", Description = "Создание цифровых решений для оптимизации процессов." },
                new Service { Name = "Консалтинг", Description = "Анализ и разработка IT-стратегий для вашего бизнеса." }
            };

            return View(services);
        }
    }
}
