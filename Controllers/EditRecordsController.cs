using BirthdayCalendarMVC.Models;
using BirthdayCalendarMVC.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;

namespace BirthdayCalendarMVC.Controllers
{
    public class EditRecordsController : Controller
    {
        private readonly MongoService _mongoService;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public EditRecordsController(MongoService mongoService, IWebHostEnvironment webHostEnvironment)
        {
            _mongoService = mongoService;
            _webHostEnvironment = webHostEnvironment;
        }
        public IActionResult Index()
        {
            ViewData["Date"] = DateOnly.FromDateTime(DateTime.Today).ToString();
            ViewData["AllRecords"] = _mongoService.GetAsync().Result;
            return View();
        }

        public IActionResult Edit(string bsonId)
        {
            try
            {
                var person = _mongoService.GetAsync(bsonId).Result;
                ViewData["Person"] = person;
                return View("Edit");
            }
            catch
            {
                return RedirectToAction("Failure");
            }
        }

        [HttpPost]
        public IActionResult Edit(Person person)
        {
            var BsonId = Request.Form["BsonId"];

            var oldPerson = _mongoService.GetAsync(BsonId).Result;

            try
            {
                if (true)
                {
                    PersonDTO personDTO = new PersonDTO
                    {
                        Date = !string.IsNullOrEmpty(person.Date.ToString()) ? person.Date.ToLocalTime() : oldPerson.Date,
                        Name = !string.IsNullOrEmpty(person.Name) ? person.Name : oldPerson.Name,
                        BsonId = BsonId
                    };

                    personDTO.ImageUrl = ImageProcessing.StoreImage(person.Image, _webHostEnvironment).Result;
                    if (personDTO.ImageUrl == null)
                        personDTO.ImageUrl = oldPerson.ImageUrl;

                    _mongoService.UpdateAsync(BsonId, personDTO).Wait();
                    return View("Success");

                }
            }
            catch
            {
                return View("Failure");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult Delete(string bsonID)
        {
            try
            {
                _mongoService.RemoveAsync(bsonID).Wait();
            }
            catch
            {
                return RedirectToAction("Failure");
            }

            return RedirectToAction("Index");
        }

        [HttpPost]
        public IActionResult DeleteMultiple(List<string> deleteMultiple)
        {
            try
            {
                foreach (string bsonID in deleteMultiple)
                {
                    _mongoService.RemoveAsync(bsonID);
                }
            }
            catch
            {
                return RedirectToAction("Failure");
            }

            return RedirectToAction("Index");
        }
    }
}
