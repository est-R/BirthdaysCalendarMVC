using BirthdayCalendarMVC.Models;
using BirthdayCalendarMVC.Services;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayCalendarMVC.Controllers
{
    public class AllRecordsController : Controller

    {
        private readonly MongoService _mongoService;

        public AllRecordsController(MongoService mongoService)
        {
            _mongoService = mongoService;
        }
        public IActionResult Index()
        {
            ViewData["Date"] = DateOnly.FromDateTime(DateTime.Today).ToString();
            ViewData["AllRecords"] = _mongoService.GetAsync().Result;
            ViewData["Host"] = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";

            return View();
        }
    }
}
