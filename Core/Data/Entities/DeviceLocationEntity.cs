using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Base;

namespace Core.Entities {
    /// <summary>
    /// Актуальные координаты устройства
    /// </summary>
    [Table(name: "DeviceLocations")]
    public class DeviceLocationEntity: LocationEntity {
        [ForeignKey("Device")]
        public long? DeviceEntity_Id { get; set; }
        public virtual DeviceEntity Device { get; set; }
    }
}
