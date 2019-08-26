using Core.Dto;
using Core.Services.Business;
using Microsoft.AspNetCore.SignalR;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SmartWatch.Hubs {
    public interface IDeviceLocationHub {
        Task SendAsync(string method, object args);
    }

    public class DeviceLocationHub : Hub {
        public readonly ILocationBusinessService LocationService;

        public DeviceLocationHub(ILocationBusinessService locationBusinessService) {
            LocationService = locationBusinessService;
        }

        public async Task ChangeDevicesLocation(List<DeviceLocationDto> devices) {
            await Clients.All.SendAsync("changeDevicesLocation", devices);
        }

        //public async Task ChangeDevicesLocation(string connectionId, List<DeviceLocationDto> devices) {
        //    await Clients.Client(connectionId).SendAsync("changeDevicesLocation", devices);
        //}

        public async Task SendNotification(string connectionId, string message) {
            await Clients.All.SendAsync("sendNotification", message);
        }

        public override async Task OnConnectedAsync() {
            var connectionId = Context.ConnectionId;

            var result = await LocationService.GetDevicesLastLocation();
            await ChangeDevicesLocation(result.ToList());
            await base.OnConnectedAsync();
        }
    }
}
