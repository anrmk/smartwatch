using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Managers {
    public interface IDeviceManager: IEntityService<DeviceEntity> {
        Task<DeviceEntity> FindByImei(string imei);
        Task<DeviceEntity> FindInclude(long id);
    }

    public class DeviceManager: AsyncEntityService<DeviceEntity>, IDeviceManager {
        public DeviceManager(IApplicationContext context) : base(context) { }

        public async Task<DeviceEntity> FindByImei(string imei) {
            return await DbSet.Where(x => x.Imei == imei).FirstOrDefaultAsync();
        }

        public async Task<DeviceEntity> FindInclude(long id) {
            return await DbSet.Where(x => x.Id == id)
                .Include(x => x.Locations)
                .FirstOrDefaultAsync();
        }
    }
}
