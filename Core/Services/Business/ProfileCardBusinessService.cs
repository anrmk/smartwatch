using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using AutoMapper;
using Core.Dto;
using Core.Entities;
using Core.Extensions;
using Core.Managers;
using Core.Models;

namespace Core.Services.Business {
    public interface IProfileCardBusinessService {
        Task<Pager<ProfileCardDtoList>> GetProfileCardPager(string search, string sort, string order, int offset, int limit);
        Task<IEnumerable<ProfileCardDtoList>> GetProfileCards();
        Task<ProfileCardDto> GetProfileCard(long id);
        Task<ProfileCardDto> CreateProfileCard(string username, ProfileCardGeneralDto dto);
        Task<ProfileCardDto> UpdateProfileCardGeneral(string username, long id, ProfileCardGeneralDto dto);
        Task<ProfileCardDto> UpdateProfileCardAdditional(string username, long id, ProfileCardAdditionalDto dto);
        Task<bool> DeleteDevice(long id);
        //Task<ProfileCardDto> AddDeviceToProfile(string username, long profileId, long deviceId);
        //Task<ProfileCardDto> RemoveDeviceFromProfile(string userName, long profileId, long deviceId);

        Task<ProfileCardMediaDto> AddProfileCardMedia(string username, long profileId, ProfileCardMediaDto dto);
        Task<bool> DeleteProfileCardMedia(long id);
    }

    public class ProfileCardBusinessService: IProfileCardBusinessService {
        private readonly IProfileCardManager profileCardManager;
        private readonly IProfileCardMediaManager profileCardMediaManager;
        private readonly IDeviceManager deviceManager;

        public ProfileCardBusinessService(IProfileCardManager profileCardManager, IProfileCardMediaManager profileCardMediaManager, IDeviceManager deviceManager) {
            this.profileCardManager = profileCardManager;
            this.profileCardMediaManager = profileCardMediaManager;
            this.deviceManager = deviceManager;
        }

        public async Task<ProfileCardDto> CreateProfileCard(string username, ProfileCardGeneralDto dto) {
            var entity = Mapper.Map<ProfileCardEntity>(dto);
            var result = await profileCardManager.Create(entity);
            return Mapper.Map<ProfileCardDto>(result);
        }

        public async Task<ProfileCardDto> GetProfileCard(long id) {
            var result = await profileCardManager.FindInclude(id);
            var dto = Mapper.Map<ProfileCardDto>(result);
            return dto;
        }

        public async Task<Pager<ProfileCardDtoList>> GetProfileCardPager(string search, string sort, string order, int offset, int limit) {
            Expression<Func<ProfileCardEntity, bool>> where = x =>
                   (true)
                && (x.Name.Contains(search) || x.Surname.Contains(search) || x.Middlename.Contains(search) || x.PhoneNumber.Contains(search));

            var sortby = ExpressionExtension.GetExpression<ProfileCardEntity>(sort ?? "Name");

            Tuple<List<ProfileCardEntity>, int> tuple = await profileCardManager.Pager<ProfileCardEntity>(where, sortby, !order.Equals("asc"), offset, limit);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if(count == 0)
                return new Pager<ProfileCardDtoList>(new List<ProfileCardDtoList>(), 0, offset, limit);

            var page = (offset + limit) / limit;

            var result = Mapper.Map<List<ProfileCardDtoList>>(list);
            return new Pager<ProfileCardDtoList>(result, count, page, limit);
        }

        public async Task<IEnumerable<ProfileCardDtoList>> GetProfileCards() {
            var result = await profileCardManager.All();
            return Mapper.Map<List<ProfileCardDtoList>>(result);
        }

        public async Task<ProfileCardDto> UpdateProfileCardGeneral(string username, long id, ProfileCardGeneralDto dto) {
            var item = await profileCardManager.Find(id);
            var item2 = Mapper.Map(dto, item);

            item = await profileCardManager.UpdateType(item2);
            return Mapper.Map<ProfileCardDto>(item);
        }

        public async Task<ProfileCardDto> UpdateProfileCardAdditional(string username, long id, ProfileCardAdditionalDto dto) {
            var item = await profileCardManager.FindInclude(id);
            var item2 = Mapper.Map(dto, item);
            //item2.Device.Clear();

            item = await profileCardManager.UpdateType(item2);

            //if(dto.DeviceId != null && dto.DeviceId.Count > 0) {
            //    foreach(var d in dto.DeviceId) {
            //        var device = await deviceManager.Find(d.Id);

            //        if(device != null) {
            //            device.ProfileCardEntityId = device.Id;
            //            await deviceManager.Update(device);
            //           // item.Device.Add(device);
            //        }
            //    }
            //}

            return Mapper.Map<ProfileCardDto>(item);
        }

        public async Task<ProfileCardMediaDto> CreateApbPersonMedia(string username, ProfileCardMediaDto dto) {
            var item = Mapper.Map<ProfileCardMediaEntity>(dto);
            item = await profileCardMediaManager.Create(item);

            return Mapper.Map<ProfileCardMediaDto>(item);
        }

        public async Task<bool> DeleteDevice(long id) {
            var item = await profileCardMediaManager.Find(id);
            if(item != null) {
                var result = await profileCardMediaManager.Delete(item);
                return result != 0;
            }
            return false;
        }

        //public async Task<ProfileCardDto> AddDeviceToProfile(string username, long profileId, long deviceId) {
        //    var itemProfile = await profileCardManager.Find(profileId);
        //    if (itemProfile != null) {
        //        var itemDevice = await deviceManager.Find(deviceId);
        //        if (itemDevice != null && !itemProfile.Device.Contains(itemDevice)) {
        //            itemProfile.Device.Add(itemDevice);
        //            itemProfile = await profileCardManager.UpdateType(itemProfile);
        //            return Mapper.Map<ProfileCardDto>(itemProfile);
        //        }
        //    }
        //    return null;
        //}

        //public async Task<ProfileCardDto> RemoveDeviceFromProfile(string userName, long profileId, long deviceId) {
        //    var itemProfile = await profileCardManager.Find(profileId);
        //    if (itemProfile != null) {
        //        var itemDevice = await deviceManager.Find(deviceId);
        //        if (itemDevice != null && !itemProfile.Device.Contains(itemDevice)) {
        //            itemProfile.Device.Remove(itemDevice);
        //            itemProfile = await profileCardManager.UpdateType(itemProfile);
        //            return Mapper.Map<ProfileCardDto>(itemProfile);
        //        }
        //    }
        //    return null;
        //}

        public async Task<ProfileCardMediaDto> AddProfileCardMedia(string username, long profileId, ProfileCardMediaDto dto) {
            var itemProfile = await profileCardManager.FindInclude(profileId);
            if(itemProfile != null) {
                var mediaProfile = Mapper.Map<ProfileCardMediaEntity>(dto);
                mediaProfile = await profileCardMediaManager.Create(mediaProfile);
                return Mapper.Map<ProfileCardMediaDto>(mediaProfile);
            }
            return null;
        }

        public async Task<bool> DeleteProfileCardMedia(long id) {
            var item = await profileCardMediaManager.Find(id);
            if(item != null) {
                var result = await profileCardMediaManager.Delete(item);
                return result != 0;
            }
            return false;
        }
    }
}
