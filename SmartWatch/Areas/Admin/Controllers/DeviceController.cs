using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto;
using Core.Services.Business;
using Microsoft.AspNetCore.Mvc;
using SmartWatch.Areas.Admin.Data.ViewModels;

namespace SmartWatch.Areas.Admin.Controllers {
    [Area("admin")]
    [Route("[area]/[controller]/")]
    public class DeviceController: Controller {
        private readonly ILocationBusinessService locationService;

        public DeviceController(ILocationBusinessService locationBusinessService) {
            locationService = locationBusinessService;
        }

        [HttpGet("create", Name = "CreateDevice")]
        public IActionResult Create() {
            var model = new DeviceViewModel();
            return View("Edit", model);
        }

        [HttpGet("edit/{id}", Name = "EditDevice")]
        public async Task<IActionResult> Edit(long? id) {
            if(id == null) {
                return NotFound();
            }

            var item = await locationService.GetDevice(id ?? 0);
            if(item == null) {
                return NotFound();
            }
            return View(Mapper.Map<DeviceViewModel>(item));
        }
    }
}

namespace SmartWatch.Areas.Admin.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class DeviceController: ControllerBase {
        private readonly ILocationBusinessService locationService;

        public DeviceController(ILocationBusinessService locationBusinessService) {
            locationService = locationBusinessService;
        }

        [HttpGet("list", Name = "ListDeviceApi")]
        public async Task<IActionResult> GetDevices() {
            var result = await locationService.GetDevices();
            return Ok(result.ToList());
        }

        [HttpPost("create", Name = "CreateDeviceApi")]
        public async Task<IActionResult> CreateDevice(DeviceViewModel model) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var result = await locationService.CreateDevice("", Mapper.Map<DeviceDto>(model));
            return Ok(result);
        }

        [HttpPut("update/{id}", Name = "UpdateDeviceApi")]
        public async Task<IActionResult> UpdateDevice(long id, DeviceViewModel model) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }

            var result = await locationService.GetDevice(id);
            if(result == null) {
                NotFound();
            }

            var item = await locationService.UpdateDevice("", id, Mapper.Map<DeviceDto>(model));
            return Ok(Mapper.Map<DeviceDto>(item));
        }

        [HttpDelete("delete/{id}", Name = "DeleteDeviceApi")]
        public async Task<IActionResult> DeleteDevices(long id) {
            var result = await locationService.DeleteDevice(id);
            return Ok(result);
        }
    }
}