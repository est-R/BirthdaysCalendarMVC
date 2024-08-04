using BirthdayCalendarMVC.Models;
using BirthdayCalendarMVC.Services;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace BirthdayCalendarMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly MongoService _mongoService;

        public HomeController(MongoService mongoService)
        {
            _mongoService = mongoService;
        }

        public IActionResult Index()
        {
            ViewData["Date"] = DateOnly.FromDateTime(DateTime.Today).ToString();
            ViewData["Todays"] = _mongoService.GetTodaysPersons();
            ViewData["Nearest"] = _mongoService.GetNearestPersons();
            ViewData["Host"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
