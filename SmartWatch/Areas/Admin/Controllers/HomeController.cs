using System;
using Microsoft.AspNetCore.Mvc;
using SmartWatch.Areas.Admin.Data.ViewModels;

namespace SmartWatch.Areas.Admin.Controllers {
    [Area("admin")]
    //[Route("[aria]/[controller]")]
    public class HomeController: Controller {
        [Route("[area]/[controller]/index")]
        public IActionResult Index() {
            var item = new IndicatorViewModel() {
                StartFrom = DateTime.Now.AddDays(-1),
                EndTill = DateTime.Now.AddHours(6)
            };

            return View(item);
        }

        [HttpGet("[area]/[controller]/profiles", Name = "ListProfileCard")]
        public IActionResult Profiles() {
            return View();
        }

        [HttpGet("[area]/[controller]/devices", Name = "ListDevice")]
        public IActionResult Devices() {
            return View();
        }
    }
}