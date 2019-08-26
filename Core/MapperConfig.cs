using AutoMapper;
using Core.Dto;
using Core.Entities;

namespace Core {
    public class MapperConfig : Profile {
        public MapperConfig() {
            CreateMap<DeviceEntity, DeviceDto>().ReverseMap();
            CreateMap<DeviceLocationEntity, DeviceLocationDto>()
                .ForMember(d => d.DeviceId, o => o.MapFrom(s => s.DeviceEntity_Id))
                .ForMember(d => d.DeviceName, o => o.MapFrom(s => s.Device.Name))
                .ReverseMap()
                .ForMember(d => d.DeviceEntity_Id, o => o.MapFrom(s => s.DeviceId));
            CreateMap<DeviceLastLocationEntity, DeviceLocationDto>()
                .ForMember(d => d.DeviceId, o => o.MapFrom(s => s.DeviceEntity_Id))
                .ForMember(d => d.DeviceName, o => o.MapFrom(s => s.Device.Name))
                .ReverseMap()
                .ForMember(d => d.DeviceEntity_Id, o => o.MapFrom(s => s.DeviceId));
        }
    }
}