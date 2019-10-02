namespace Core.Dto {
    public class DeviceDto {
        public long Id { get; set; }
        public string Imei { get; set; }
        public string Name { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }
        public string Version { get; set; }
        public string Platform { get; set; }
        public string Idiom { get; set; }
        //public DeviceLocationDto Location { get; set; }
        public int Status { get; set; }

        public ProfileCardDto Profile { get; set; }
        //public string ProfileImage { get; set; }
        // public ProfileCardMediaDto Media { get; set; }
    }
}
