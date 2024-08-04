using BirthdayCalendarMVC.Models;
using BirthdayCalendarMVC.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Runtime.CompilerServices;
using System;
using Microsoft.AspNetCore.Hosting;

namespace BirthdayCalendarMVC.Controllers
{
    public class AddRecordController : Controller
    {
        private readonly MongoService _mongoService;
        private readonly IWebHostEnvironment _webHostEnvironment;


        public AddRecordController(MongoService mongoService, IWebHostEnvironment webHostEnvironment)
        {
            _mongoService = mongoService;
            _webHostEnvironment = webHostEnvironment;

        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Add(Person person)
        {
            try
            {
                if (ModelState.IsValid)
                {
                    PersonDTO personDTO = new PersonDTO
                    {
                        Date = person.Date.ToLocalTime(),
                        Name = person.Name
                    };

                    personDTO.ImageUrl = ImageProcessing.StoreImage(person.Image, _webHostEnvironment).Result;
                    _mongoService.CreateAsync(personDTO);
                    return RedirectToAction("Success");

                }
            }
            catch
            {
                return RedirectToAction("Failure");
            }

            return RedirectToAction("Index");
        }
        public IActionResult Success()
        {
            return View();
        }

        public IActionResult Failure()
        {
            return View();
        }
    }
}
