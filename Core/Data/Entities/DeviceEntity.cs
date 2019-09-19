using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Base;
using Core.Enums;

namespace Core.Entities {
    /// <summary>
    /// Оборудование (планшеты)
    /// </summary>
    [Table(name: "Devices")]
    public class DeviceEntity: AuditableEntity<long> {
        /// <summary>
        /// Device IMEI code 
        /// </summary>
        [MaxLength(24)]
        public string Imei { get; set; }

        /// <summary>
        /// Device Model (SMG-950U, iPhone10,6)
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Manufacturer (Samsung)
        /// </summary>
        public string Manufacturer { get; set; }

        /// <summary>
        /// Operating System Version Number (7.0)
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Platform (Android)
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Idiom (Phone/ Tablet)
        /// </summary>
        public string Idiom { get; set; }

        [ForeignKey("ProfileCard")]
        public long? ProfileCardEntityId { get; set; }
        public virtual ProfileCardEntity ProfileCard { get; set; }

        /// <summary>
        /// Координаты оборудования
        /// </summary>
        public virtual ICollection<DeviceLocationEntity> Locations { get; set; }

        public DeviceStatusEnum Status { get; set; }
    }
}
