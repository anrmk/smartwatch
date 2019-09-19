using System;
using System.Threading.Tasks;
using Core.Dto;
using Core.Managers;

namespace Core.Services.Business {
    public interface IIndicatorBusinessService {
        Task<ChartSeriesDto<double>> GetRegisteredDevices(DateTime start, DateTime end);
        Task<ChartSeriesDto<object[]>> GetDeviceAccuracyInfo(long id, DateTime start, DateTime end);
    }

    public class IndicatorBusinessService: IIndicatorBusinessService {
        private readonly IIndicatorManager indicatorManager;

        public IndicatorBusinessService(IIndicatorManager indicatorManager) {
            this.indicatorManager = indicatorManager;
        }

        public async Task<ChartSeriesDto<double>> GetRegisteredDevices(DateTime start, DateTime end) {
            return await indicatorManager.GetRegisteredDevices(start, end);
        }

        public async Task<ChartSeriesDto<object[]>> GetDeviceAccuracyInfo(long id, DateTime start, DateTime end) {
            return await indicatorManager.GetDeviceAccuracyInfo(id, start, end);

        }
    }
}
