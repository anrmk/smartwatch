using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto;
using Core.Services.Business;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using SmartWatch.Hubs;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SmartWatch.Controllers {
    [Route("api/[controller]")]
    public class DeviceLocationController : Controller {
        public readonly ILocationBusinessService LocationService;
        //public readonly IDeviceLocationHub DeviceLocationHub;
        private readonly IHubContext<DeviceLocationHub> DeviceLocationHubContext;

        public DeviceLocationController(IHubContext<DeviceLocationHub> deviceLocationHubContext,  ILocationBusinessService locationBusinessService) {
            LocationService = locationBusinessService;
            DeviceLocationHubContext = deviceLocationHubContext;
        }

        [HttpPost]
        //[ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Post([FromBody]RequestDto request) {
            var sw = Stopwatch.StartNew();
            var device = request.Device;
            var location = request.Location;
            //var locationDto = request.GetParamsAsObject<DeviceLocationDto>();

            var item = await LocationService.InsertDeviceLocation(device, location);
            sw.Stop();

            if (item != null) {
                await DeviceLocationHubContext.Clients.All.SendAsync("changeDevicesLocation", new List<DeviceLocationDto>() { item });
            }

            return Ok(new ResponseDto() {
                LeadTime = sw.Elapsed,
                Message = (item == null || item?.Id > 0) ? "Resources.Resource.MessageNotFound" : "Resources.Resource.MessageRegCoordinate"
            });

        }

        // GET: api/<controller>
        [HttpGet]
        [Route("devices")]
        public async Task<IActionResult> GetDevices() {
            var result = await LocationService.GetDevices();
            return Ok(result.ToList());
        }

        [HttpGet]
        [Route("{id}/locations")]
        public async Task<IActionResult> GetLocations(long id) {
            var result = await LocationService.GetDevicesLocation(id);
            return Ok(result.ToList());
        }

        // GET api/<controller>/5
        [HttpGet("{id}")]
        public string Get(int id) {
            return "value";
        }

        // POST api/<controller>
        //[HttpPost]
        //public void Post([FromBody]string value) {
        //}

        // PUT api/<controller>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody]string value) {
        }

        // DELETE api/<controller>/5
        [HttpDelete("{id}")]
        public void Delete(int id) {
        }
    }
}
