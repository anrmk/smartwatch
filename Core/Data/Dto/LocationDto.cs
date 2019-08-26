using System;
using System.Collections.Generic;
using System.Text;

namespace Core.Dto {
    public class LocationDto {
        public double Latitude { get; set; }
        public double Longitude { get; set; }
        public double Altitude { get; set; }
        public double Accuracy { get; set; }
        public double Speed { get; set; }
        public double Direction { get; set; }
        public bool IsFromMockProvider { get; set; }
        public DateTimeOffset Timestamp { get; set; }
    }
}
