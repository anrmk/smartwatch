using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using SmartWatch.Areas.Admin.Data.ViewModels;
using SmartWatch.Models;

namespace SmartWatch.Controllers {
    public class HomeController: Controller {
        public IActionResult Index() {
            var item = new IndicatorViewModel() {
                StartFrom = DateTime.Now.AddDays(-1),
                EndTill = DateTime.Now.AddHours(6)
            };
            return View(item);
        }

        public IActionResult Privacy() {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error() {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
