using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Managers {
    public interface IProfileCardMediaManager: IEntityService<ProfileCardMediaEntity> {
        Task<ProfileCardMediaEntity> FindInclude(long id);
        Task<List<ProfileCardMediaEntity>> FindByIds(long[] ids);
    }

    public class ProfileCardMediaManager: AsyncEntityService<ProfileCardMediaEntity>, IProfileCardMediaManager {
        public ProfileCardMediaManager(IApplicationContext context) : base(context) {

        }
        public async Task<List<ProfileCardMediaEntity>> FindByIds(long[] ids) {
            return await DbSet
                .Where(x => ids.Contains(x.Id)).ToListAsync();
        }

        public async Task<ProfileCardMediaEntity> FindInclude(long id) {
            return await DbSet
                .Where(x => x.Id == id).FirstOrDefaultAsync();
        }
    }
}
