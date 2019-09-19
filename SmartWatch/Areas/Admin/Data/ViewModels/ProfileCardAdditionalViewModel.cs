using System.ComponentModel.DataAnnotations;

namespace SmartWatch.Areas.Admin.Data.ViewModels {
    public class ProfileCardAdditionalViewModel {
        public long Id { get; set; }

        [Range(90, 250)]
        public int Systolic { get; set; }

        [Range(60, 140)]
        public int Diastolic { get; set; }

        public long? DeviceId { get; set; }
    }
}
