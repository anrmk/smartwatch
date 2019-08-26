
namespace Core.Dto {
    public class DeviceLocationDto : LocationDto {
        public long Id { get; set; }
        public string DeviceName { get; set; }

        public long? DeviceId { get; set; }
    }
}
