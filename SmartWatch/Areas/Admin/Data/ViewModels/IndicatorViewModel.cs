using System;
using System.ComponentModel.DataAnnotations;

namespace SmartWatch.Areas.Admin.Data.ViewModels {
    public class IndicatorViewModel {
        [Required]
        public DateTime StartFrom { get; set; }

        [Required]
        public DateTime EndTill { get; set; }
    }
}
