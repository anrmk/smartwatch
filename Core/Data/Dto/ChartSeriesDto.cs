
using System.Collections.Generic;

namespace Core.Dto {
    public class ChartSeriesDto<T> {
        public List<string> Categories { get; set; }
        public List<ChartDataSeriesDto<T>> Series { get; set; }
    }

    public class ChartDataSeriesDto<T> {
        public string Name { get; set; }
        public T[] Data { get; set; }
    }
}
