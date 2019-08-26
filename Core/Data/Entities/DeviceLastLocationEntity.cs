using Core.Entities.Base;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core.Entities {
    [Table("DeviceLastLocations")]
    public class DeviceLastLocationEntity : LocationEntity {
        [ForeignKey("Device")]
        public long? DeviceEntity_Id { get; set; }
        public virtual DeviceEntity Device { get; set; }
    }
}
