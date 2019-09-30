using System;
using AutoMapper;
using Core.Dto;
using Core.Entities;

namespace Core {
    public class MapperConfig: Profile {
        public MapperConfig() {
            CreateMap<DeviceEntity, DeviceDto>()
                //.ForMember(d => d.ProfileName, o => o.MapFrom(s => s.ProfileCard != null ? string.Format("{0} {1} {2}", s.ProfileCard.Name, s.ProfileCard.Surname, s.ProfileCard.Middlename) : s.Name))
                .ForMember(d => d.Profile, o => o.MapFrom(s => s.ProfileCard))
                .ReverseMap();
            CreateMap<DeviceLocationEntity, DeviceLocationDto>()
                .ForMember(d => d.DeviceId, o => o.MapFrom(s => s.DeviceEntity_Id))
                .ForMember(d => d.DeviceName, o => o.MapFrom(s => s.Device.Name))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s =>  s.Timestamp.ToUnixTimeMilliseconds()))
                .ReverseMap()
                .ForMember(d => d.DeviceEntity_Id, o => o.MapFrom(s => s.DeviceId))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => DateTimeOffset.FromUnixTimeMilliseconds(s.Timestamp)));

            CreateMap<DeviceLastLocationEntity, DeviceLocationDto>()
                .ForMember(d => d.DeviceId, o => o.MapFrom(s => s.DeviceEntity_Id))
                .ForMember(d => d.DeviceName, o => o.MapFrom(s => s.Device.Name))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => s.Timestamp.ToUnixTimeMilliseconds()))
                .ReverseMap()
                .ForMember(d => d.DeviceEntity_Id, o => o.MapFrom(s => s.DeviceId))
                .ForMember(d => d.Timestamp, o => o.MapFrom(s => DateTimeOffset.FromUnixTimeMilliseconds(s.Timestamp)));

            CreateMap<ProfileCardEntity, ProfileCardDtoList>()
                .ForMember(d => d.BirthDate, o => o.MapFrom(s => s.BirthDate.ToString("dd.MM.yyyy")));
            CreateMap<ProfileCardEntity, ProfileCardDto>()
                .ForMember(d => d.General, o => o.MapFrom(s => new ProfileCardGeneralDto() {
                    Name = s.Name,
                    Surname = s.Surname,
                    Middlename = s.Middlename,
                    PhoneNumber = s.PhoneNumber,
                    Address = s.Address,
                    BirthDate = s.BirthDate
                }))
                .ForMember(d => d.Additional, o => o.MapFrom(s => new ProfileCardAdditionalDto() {
                    Systolic = s.Systolic,
                    Diastolic = s.Diastolic,
                    DeviceId = s.DeviceEntity_Id
                }))
               .ForMember(d => d.Medias, o => o.MapFrom(s => s.Medias));

            CreateMap<ProfileCardGeneralDto, ProfileCardEntity>();
            CreateMap<ProfileCardAdditionalDto, ProfileCardEntity>()
                .ForMember(d => d.DeviceEntity_Id, o => o.MapFrom(s => s.DeviceId))
                .ReverseMap()
                .ForMember(d => d.DeviceId, o => o.MapFrom(s => s.DeviceEntity_Id));
            CreateMap<ProfileCardMediaDto, ProfileCardMediaEntity>()
                .ForMember(d => d.ProfileCardId, o => o.MapFrom(s => s.ProfileCardId));


        }
    }
}