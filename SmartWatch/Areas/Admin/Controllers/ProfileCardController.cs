using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto;
using Core.Services.Business;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using SmartWatch.Areas.Admin.Data.ViewModels;

namespace SmartWatch.Areas.Admin.Controllers {
    [Area("admin")]
    [Route("[area]/[controller]/")]
    public class ProfileCardController: Controller {
        private readonly IProfileCardBusinessService profileCardBusinessService;
        private readonly ILocationBusinessService locationService;

        public ProfileCardController(IProfileCardBusinessService profileCardBusinessService, ILocationBusinessService locationService) {
            this.profileCardBusinessService = profileCardBusinessService;
            this.locationService = locationService;
        }

        [HttpGet("create", Name = "CreateProfileCard")]
        public async Task<IActionResult> Create() {
            var model = new ProfileViewModel();

            // devices
            var devices = await locationService.GetDevices();
            var deviceList = new List<SelectListItem>();
            deviceList.Add(new SelectListItem { Value = "", Text = "Выберите значение" });
            deviceList.AddRange(devices.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Imei }).ToList());
            ViewBag.Devices = deviceList;


            return View("Edit", model);
        }

        [HttpGet("edit/{id}", Name = "EditProfileCard")]
        public async Task<IActionResult> Edit(long? id) {
            if(id == null) {
                return NotFound();
            }

            var item = await profileCardBusinessService.GetProfileCard(id ?? 0);
            if(item == null) {
                return NotFound();
            }

            // devices
            var devices = await locationService.GetDevices();
            var deviceList = new List<SelectListItem>();
            deviceList.Add(new SelectListItem { Value = "", Text = "Выберите значение" });
            deviceList.AddRange(devices.Select(x => new SelectListItem { Value = x.Id.ToString(), Text = x.Imei }).ToList());
            ViewBag.Devices = deviceList;

            var model = Mapper.Map<ProfileViewModel>(item);

            return View(model);
        }
    }
}

namespace SmartWatch.Areas.Admin.Controllers.Api {
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileCardController: ControllerBase {
        private readonly IProfileCardBusinessService profileCardBusinessService;

        public ProfileCardController(IProfileCardBusinessService profileCardBusinessService) {
            this.profileCardBusinessService = profileCardBusinessService;
        }

        [HttpGet]
        [Route("list", Name = "ListProfileCardApi")]
        public async Task<IActionResult> GetProfileCard() {
            var result = await profileCardBusinessService.GetProfileCards();
            return Ok(result.ToList());
        }

        [HttpPost("create", Name = "CreateProfileCardApi")]
        public async Task<IActionResult> CreateProfileCard(ProfileCardGeneralViewModel model) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var result = await profileCardBusinessService.CreateProfileCard("", Mapper.Map<ProfileCardGeneralDto>(model));
            return Ok(result);
        }

        [HttpPut("edit/{id}/general", Name = "UpdateProfileCardGeneralApi")]
        public async Task<IActionResult> UpdateProfileCardGeneral(long id, ProfileCardGeneralViewModel model) {
            if(!ModelState.IsValid) {
                return BadRequest(ModelState);
            }
            var result = await profileCardBusinessService.UpdateProfileCardGeneral("", id, Mapper.Map<ProfileCardGeneralDto>(model));
            return Ok(result);
        }

        [HttpPut("edit/{id}/additional", Name = "UpdateProfileCardAdditionalApi")]
        public async Task<IActionResult> UpdateProfileCardAdditional(long id, ProfileCardAdditionalViewModel model) {
            if(!ModelState.IsValid) {
                BadRequest(ModelState);
            }
            var dto = Mapper.Map<ProfileCardAdditionalDto>(model);
            var result = await profileCardBusinessService.UpdateProfileCardAdditional("", id, dto);
            return Ok(result);
        }

        [HttpDelete("delete/{id}", Name = "DeleteProfileCardApi")]
        public async Task<IActionResult> DeleteProfileCard(long id) {
            var result = await profileCardBusinessService.DeleteDevice(id);
            return Ok(new { Message = result ? $"Запись {id} удалена" : "Произошла ошибка при попытке удаления записи" });
        }

        #region MEDIA
        [HttpPost("{id:long}/mediaupload", Name = "UploadProfileCardMediaApi")]
        public async Task<IActionResult> MediaUpload(long id) {
            var files = Request.Form.Files;
            if(files == null || files.Count == 0) {
                return BadRequest("Файлы не загружены");
            }

            var item = await profileCardBusinessService.GetProfileCard(id);
            if(item == null) {
                return BadRequest("Не найден идентификатор");
            }
            long size = files.Sum(f => f.Length);


            var result = new List<ProfileCardMediaDto>();
            foreach(var formFile in files) {
                if(formFile.Length > 0) {
                    using(var memoryStream = new MemoryStream()) {
                        await formFile.CopyToAsync(memoryStream, new System.Threading.CancellationToken());

                        var pitem = await profileCardBusinessService.AddProfileCardMedia("", id, new ProfileCardMediaDto() {
                            ProfileCardId = id,
                            Source = memoryStream.ToArray(),
                            ContentType = formFile.ContentType,
                            Name = formFile.Name
                        });
                        if(pitem != null) {
                            result.Add(pitem);
                        }
                    }
                }
            }
            return Ok(result);
        }

        [HttpDelete("media/{id:long}", Name = "DeleteProfileCardMediaApi")]
        public async Task<IActionResult> DeleteImage(long id) {
            var result = await profileCardBusinessService.DeleteDevice(id);
            return Ok(new { Message = result ? $"Запись {id} удалена" : "Произошла ошибка при попытке удаления записи" });
        }
        #endregion
    }
}