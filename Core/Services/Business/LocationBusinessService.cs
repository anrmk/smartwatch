using Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Core.Dto;
using Core.Models;
using AutoMapper;
using Core.Managers;
using Core.Extensions;

namespace Core.Services.Business {
    public interface ILocationBusinessService {
        Task<Pager<DeviceDto>> GetDevicePager(string search, string sort, string order, int offset, int limit);
        Task<DeviceDto> GetDevice(long id);
        Task<DeviceDto> GetDevice(string imei);

        Task<IEnumerable<DeviceDto>> GetDevices();
        Task<DeviceDto> CreateDevice(string username, DeviceDto dto);
        Task<DeviceDto> UpdateDevice(string username, long id, DeviceDto dto);

        Task<bool> DeleteDevice(long id);
        Task<DeviceLocationDto> InsertDeviceLocation(DeviceDto deviceDto, DeviceLocationDto locationDto);

        Task<IEnumerable<DeviceLocationDto>> GetDevicesLocation(long id);
        Task<IEnumerable<DeviceLocationDto>> GetDevicesLastLocation();
    }

    public class LocationBusinessService : ILocationBusinessService {
        private readonly IDeviceManager deviceManager;
        private readonly IDeviceLocationManager deviceLocationManager;
        private readonly IDeviceLastLocationManager deviceLastLocationManager;

        public LocationBusinessService(IDeviceManager deviceService, IDeviceLocationManager deviceLocationManager, IDeviceLastLocationManager deviceLastLocationManager) {
            this.deviceManager = deviceService;
            this.deviceLocationManager = deviceLocationManager;
            this.deviceLastLocationManager = deviceLastLocationManager;
        }

        #region DEVICES
        public async Task<DeviceDto> GetDevice(long id) {
            var result = await deviceManager.FindInclude(id);
            return Mapper.Map<DeviceDto>(result);
        }

        public async Task<DeviceDto> GetDevice(string imei) {
            var result = await deviceManager.FindByImei(imei);
            return Mapper.Map<DeviceDto>(result);
        }

        public async Task<IEnumerable<DeviceDto>> GetDevices() {
            var result = await deviceManager.All();
            return Mapper.Map<List<DeviceDto>>(result);
        }

        public async Task<Pager<DeviceDto>> GetDevicePager(string search, string sort, string order, int offset, int limit) {
            Expression<Func<DeviceEntity, bool>> where = x =>
                   (true)
                && (x.Name.Contains(search) || x.Manufacturer.Contains(search) || x.Platform.Contains(search));

            var sortby = ExpressionExtension.GetExpression<DeviceEntity>(sort ?? "Name");

            Tuple<List<DeviceEntity>, int> tuple = await deviceManager.Pager<DeviceEntity>(where, sortby, !order.Equals("asc"), offset, limit);
            var list = tuple.Item1;
            var count = tuple.Item2;

            if (count == 0)
                return new Pager<DeviceDto>(new List<DeviceDto>(), 0, offset, limit);

            var page = (offset + limit) / limit;

            var result = Mapper.Map<List<DeviceDto>>(list);
            return new Pager<DeviceDto>(result, count, page, limit);
        }

        public async Task<DeviceDto> CreateDevice(string username, DeviceDto dto) {
            var result = await deviceManager.Create(Mapper.Map<DeviceEntity>(dto));
            return Mapper.Map<DeviceDto>(result);
        }

        public async Task<DeviceDto> UpdateDevice(string username, long id, DeviceDto dto) {
            var item = await deviceManager.FindInclude(id);
            var item2 = Mapper.Map(dto, item);

            item = await deviceManager.UpdateType(item2);
            return Mapper.Map<DeviceDto>(item);
        }

        public async Task<bool> DeleteDevice(long id) {
            var item = await deviceManager.FindInclude(id);
            if (item != null) {
                var result = await deviceManager.Delete(item);
                return result != 0;
            }
            return false;
        }
        #endregion

        #region DEVICE LOCATION
        /// <summary>
        /// Получение координат устройств
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DeviceLocationDto>> GetDevicesLocation(long id) {
            var result = await deviceLocationManager.FindById(id);
            return Mapper.Map<List<DeviceLocationDto>>(result);
        }

        /// <summary>
        /// Получение списка всех устройств с последними зафиксированными данными
        /// </summary>
        /// <returns></returns>
        public async Task<IEnumerable<DeviceLocationDto>> GetDevicesLastLocation() {
            var result = await deviceLastLocationManager.FindAll();
            return Mapper.Map<List<DeviceLocationDto>>(result);
        }

        #endregion

        /// <summary>
        /// Регистрирует позицию устройства
        /// </summary>
        /// <param name="deviceLocationDto"></param>
        /// <returns>true - координаты зарегистрированы; false - данные не зарегистрированны</returns>
        public async Task<DeviceLocationDto> InsertDeviceLocation(DeviceDto deviceDto, DeviceLocationDto deviceLocationDto) {
            var deviceItem = await deviceManager.FindByImei(deviceDto.Imei);
            if (deviceItem == null) {
                deviceItem = await deviceManager.Create(Mapper.Map<DeviceEntity>(deviceDto));
                if (deviceItem == null)
                    return null;
            }

            deviceLocationDto.DeviceId = deviceItem.Id;
            var deviceLocationItem = Mapper.Map<DeviceLocationEntity>(deviceLocationDto);

            deviceLocationItem = await deviceLocationManager.Create(deviceLocationItem);
            return Mapper.Map<DeviceLocationDto>(deviceLocationItem);
        }
    }
}
