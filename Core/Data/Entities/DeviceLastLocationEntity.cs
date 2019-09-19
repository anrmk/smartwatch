using System.ComponentModel.DataAnnotations.Schema;
using Core.Entities.Base;

namespace Core.Entities {
    [Table("DeviceLastLocations")]
    public class DeviceLastLocationEntity: LocationEntity {
        [ForeignKey("Device")]
        public long? DeviceEntity_Id { get; set; }
        public virtual DeviceEntity Device { get; set; }
    }
}
