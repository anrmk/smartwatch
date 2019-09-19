using AutoMapper;
using Core.Dto;
using SmartWatch.Areas.Admin.Data.ViewModels;

namespace SmartWatch.App_Config {
    public class MapperConfig: Profile {
        public static void Register() {
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new Core.MapperConfig());
                cfg.AddProfile(new MapperConfig());
            });
        }

        public MapperConfig() {
            CreateMap<DeviceViewModel, DeviceDto>();

            CreateMap<ProfileViewModel, ProfileCardDto>();
            CreateMap<ProfileCardGeneralViewModel, ProfileCardGeneralDto>();
            CreateMap<ProfileCardAdditionalViewModel, ProfileCardAdditionalDto>();

            CreateMap<ProfileCardMediaViewModel, ProfileCardMediaDto>()
                .ForMember(d => d.Thumbnail, o => o.MapFrom(s => s.Source))
                .ForMember(d => d.ThumbnailSrc, o => o.MapFrom(s => s.Src));
        }
    }
}
