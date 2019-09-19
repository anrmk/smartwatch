using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Managers {
    public interface IDeviceLocationManager: IEntityService<DeviceLocationEntity> {
        Task<List<DeviceLocationEntity>> FindById(long id);
    }

    public class DeviceLocationManager: AsyncEntityService<DeviceLocationEntity>, IDeviceLocationManager {
        public DeviceLocationManager(IApplicationContext context) : base(context) {
        }

        public async Task<List<DeviceLocationEntity>> FindById(long id) {
            return await DbSet.Where(x => x.DeviceEntity_Id == id)
                .OrderBy(x => x.Timestamp)
                .ToListAsync();
        }
    }
}
