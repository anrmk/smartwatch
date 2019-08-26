using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWatch.App_Config {
    public class MapperConfig: Profile {
        public static void Register() {
            Mapper.Initialize(cfg => {
                cfg.AddProfile(new Core.MapperConfig());
                cfg.AddProfile(new MapperConfig());
            });
        }

        public MapperConfig() {

        }
    }
}
