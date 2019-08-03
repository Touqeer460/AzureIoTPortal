using AzureIOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Service
{
    public interface ISubscriptionService
    {
        Response<List<Device>> GetDevicesList();
        Response<List<Telemetries>> GetTelemetries();
        Response<Device> GetDeviceDetailAsync(string id);
        Response<DeviceTelemetry> GetDeviceTelemetries(Device device);
        Response<Device> InsertDevice(Device device);
        Response<Telemetries> InsertTelemetry(Telemetries telemetry);

        Task<Response<List<Device>>> GetDevicesListAsync();


        //Will add interface methods for rules later
    }
}
