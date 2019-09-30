using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Context;
using Core.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Managers {
    public interface IDeviceLastLocationManager: IEntityService<DeviceLastLocationEntity> {
        Task<List<DeviceLastLocationEntity>> FindAll();
    }

    public class DeviceLastLocationManager: AsyncEntityService<DeviceLastLocationEntity>, IDeviceLastLocationManager {
        public DeviceLastLocationManager(IApplicationContext context) : base(context) {
        }

        public async Task<List<DeviceLastLocationEntity>> FindAll() {
            return await DbSet
                .Include(x => x.Device)
                .Include(x => x.Device.ProfileCard)
                .ToListAsync();
        }
    }
}
