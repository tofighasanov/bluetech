using Microsoft.AspNetCore.Mvc;
using Bluetech.Models;
using System.Collections.Generic;
using System;

namespace Bluetech.Controllers
{
    public class ProjectsController : Controller
    {
        public IActionResult Index()
        {
            var items = new List<Project>
            {
                new Project {
                    Title = "ERP для логистики",
                    Customer = "ACME Logistics",
                    ShortDescription = "Внедрение модулей складского учета и интеграция с 1C API.",
                    LaunchDate = new DateTime(2024, 11, 15),
                    Link = "#",
                    ImagePath = "/img/erp.jpg",
                    Tags = new[] {"ERP","Integration","API"}
                },
                new Project {
                    Title = "CRM для отдела продаж",
                    Customer = "Retail Group",
                    ShortDescription = "Кастомные воронки, телеграм-бот, отчеты Power BI.",
                    LaunchDate = new DateTime(2025, 03, 10),
                    Link = "#",
                    ImagePath = "/img/crm.jpg",
                    Tags = new[] {"CRM","Analytics","Automation"}
                }
            };
            return View(items);
        }
    }
}
