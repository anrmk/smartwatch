using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Managers {
    public interface IDeviceManager: IEntityService<DeviceEntity> {
        Task<List<DeviceEntity>> FindAllInclude();
        Task<DeviceEntity> FindByImei(string imei);
        Task<DeviceEntity> FindInclude(long id);
    }

    public class DeviceManager: AsyncEntityService<DeviceEntity>, IDeviceManager {
        public DeviceManager(IApplicationContext context) : base(context) { }

        public async Task<List<DeviceEntity>> FindAllInclude() {
            return await DbSet
                .Include(x => x.ProfileCard)
                .Include(x => x.ProfileCard.Medias)
                .Include(x => x.Locations)
                .ToListAsync();
        }

        public async Task<DeviceEntity> FindByImei(string imei) {
            return await DbSet.Where(x => x.Imei == imei).FirstOrDefaultAsync();
        }

        public async Task<DeviceEntity> FindInclude(long id) {
            return await DbSet.Where(x => x.Id == id)
                .Include(x => x.ProfileCard)
                .Include(x => x.ProfileCard.Medias)
                .Include(x => x.Locations)
                .FirstOrDefaultAsync();
        }
    }
}
