using System.ComponentModel.DataAnnotations;

namespace SmartWatch.Areas.Admin.Data.ViewModels {
    public class IndicatorViewModel {
        [Required]
        public string StartFrom { get; set; }

        [Required]
        public string EndTill { get; set; }
    }
}
