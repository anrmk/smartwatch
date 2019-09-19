using System.Collections.Generic;

namespace SmartWatch.Areas.Admin.Data.ViewModels {
    public class ProfileViewModel {
        public long Id { get; set; }

        public ProfileCardGeneralViewModel General { get; set; }
        public ProfileCardAdditionalViewModel Additional { get; set; }
        public List<ProfileCardMediaViewModel> Medias { get; set; }
    }
}
