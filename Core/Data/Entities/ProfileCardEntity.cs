using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Base;

namespace Core.Entities {
    public class ProfileCardEntity: AuditableEntity<long> {
        [Required]
        [MaxLength(64)]
        public string Surname { get; set; }

        [MaxLength(64)]
        public string Middlename { get; set; }

        #region Blood Preasure
        public int Systolic { get; set; }
        public int Diastolic { get; set; }
        #endregion

        [DataType(DataType.Date)]
        public DateTime BirthDate { get; set; }

        [MaxLength(12)]
        public string PhoneNumber { get; set; }

        [MaxLength(512)]
        public string Address { get; set; }

        public virtual ICollection<ProfileCardMediaEntity> Medias { get; set; }

        [ForeignKey("Device")]
        public long? DeviceEntity_Id { get; set; }
        public virtual DeviceEntity Device { get; set; }
    }
}
