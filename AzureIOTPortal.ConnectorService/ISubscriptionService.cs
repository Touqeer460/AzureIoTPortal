using AzureIOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.ConnectorService
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
        Response<Rules> InsertRule(Rules rule);
        Response<List<Rules>> GetRulesList();

        Response<DeviceGroup> InsertGroup(DeviceGroup rule);
        Response<List<DeviceGroup>> GetGroupsList();

        //Will add interface methods for rules later
    }
}
