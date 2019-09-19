using System.ComponentModel.DataAnnotations;

namespace SmartWatch.Areas.Admin.Data.ViewModels {
    public class DeviceViewModel {
        [Display(Name = "Идентификатор")]
        public long Id { get; set; }

        [Required()]
        [Display(Name = "IMEI")]
        [StringLength(24)]
        public string Imei { get; set; }

        [Required()]
        [Display(Name = "Наименование")]
        [StringLength(24)]
        public string Name { get; set; }

        [Display(Name = "Модель")]
        [StringLength(24)]
        public string Model { get; set; }

        [Display(Name = "Производитель")]
        [StringLength(24)]
        public string Manufacturer { get; set; }

        [Display(Name = "Версия")]
        [StringLength(24)]
        public string Version { get; set; }

        [Display(Name = "Платформа")]
        [StringLength(24)]
        public string Platform { get; set; }

        [Display(Name = "Тип")]
        [StringLength(24)]
        public string Idiom { get; set; }

        [Display(Name = "Статус")]
        public int Status { get; set; }
    }
}
