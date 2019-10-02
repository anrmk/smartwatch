using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Managers {
    public interface IDeviceLocationManager: IEntityService<DeviceLocationEntity> {
        Task<List<DeviceLocationEntity>> FindById(long id, DateTime start, DateTime end);
    }

    public class DeviceLocationManager: AsyncEntityService<DeviceLocationEntity>, IDeviceLocationManager {
        public DeviceLocationManager(IApplicationContext context) : base(context) {
        }

        public async Task<List<DeviceLocationEntity>> FindById(long id, DateTime start, DateTime end) {
            return await DbSet.Where(x => x.DeviceEntity_Id == id && x.Timestamp >= start && x.Timestamp <= end)
                .OrderBy(x => x.Timestamp)
                .ToListAsync();
        }
    }
}
