using System.Linq;
using System.Threading.Tasks;
using Core.Context;
using Core.Entities;
using Core.Services.Base;
using Microsoft.EntityFrameworkCore;

namespace Core.Managers {
    public interface IProfileCardManager: IEntityService<ProfileCardEntity> {
        Task<ProfileCardEntity> FindInclude(long id);
    }

    public class ProfileCardManager: AsyncEntityService<ProfileCardEntity>, IProfileCardManager {
        public ProfileCardManager(IApplicationContext context) : base(context) {
        }

        public async Task<ProfileCardEntity> FindInclude(long id) {
            return await DbSet
                .Include(x => x.Device)
                .Include(x => x.Medias)
                .Where(x => x.Id == id)
                .FirstOrDefaultAsync();
        }
    }

}
