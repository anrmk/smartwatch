using System;
using System.ComponentModel.DataAnnotations;

namespace SmartWatch.Areas.Admin.Data.ViewModels {
    public class ProfileCardGeneralViewModel {
        public long Id { get; set; }

        [Required]
        [Display(Name = "Имя")]
        [StringLength(64)]
        public string Name { get; set; }

        [Required]
        [Display(Name = "Фамилия")]
        [StringLength(64)]
        public string Surname { get; set; }

        [Display(Name = "Отчество")]
        [StringLength(64)]
        public string Middlename { get; set; }

        [Display(Name = "Номер телефона")]
        [StringLength(12)]
        public string PhoneNumber { get; set; }

        [Display(Name = "Адрес")]
        [StringLength(512)]
        public string Address { get; set; }

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }
    }
}
