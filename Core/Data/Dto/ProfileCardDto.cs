using System.Collections.Generic;

namespace Core.Dto {
    public class ProfileCardDto {
        public long Id { get; set; }
        public ProfileCardGeneralDto General { get; set; }
        public ProfileCardAdditionalDto Additional { get; set; }
        public List<ProfileCardMediaDto> Medias { get; set; }
    }
}
