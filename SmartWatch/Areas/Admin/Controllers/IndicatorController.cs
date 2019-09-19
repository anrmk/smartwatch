
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Dto;
using Core.Services.Business;
using Microsoft.AspNetCore.Mvc;

namespace SmartWatch.Areas.Admin.Controllers {
    public class IndicatorController: Controller {
        public IActionResult Index() {
            return View();
        }
    }
}

namespace SmartWatch.Areas.Admin.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class IndicatorController: ControllerBase {
        private readonly IIndicatorBusinessService IndicatorService;

        public IndicatorController(IIndicatorBusinessService indicatorService) {
            IndicatorService = indicatorService;
        }

        [HttpGet]
        [Route("list", Name = "GetIndicatorsApi")]
        public async Task<IActionResult> GetIndicators() {
            var StartFrom = DateTime.Now.AddMonths(-1);
            var EndTill = DateTime.Now;
            var result = await IndicatorService.GetRegisteredDevices(StartFrom, EndTill);

            var response = new List<ChartSeriesDto<double>>() { result };

            return Ok(response);
        }

        [HttpGet]
        [Route("deviceaccuracyinfo/{id}", Name = "GetDeviceAccuracyInfoApi")]
        public async Task<IActionResult> GetDeviceAccuracyInfo(long id) {
            var StartFrom = DateTime.Now.AddMonths(-1);
            var EndTill = DateTime.Now;
            var result = await IndicatorService.GetDeviceAccuracyInfo(id, StartFrom, EndTill);

            var response = new List<ChartSeriesDto<object[]>>() { result };

            return Ok(response);
        }
    }
}