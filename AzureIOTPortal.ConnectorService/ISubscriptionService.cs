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
        DashboardFacts GetDashboardFacts();
        Device DeviceDetail(string Id);
        Response<List<Telemetries>> GetTelemetries();
        Response<Device> GetDeviceDetailAsync(string id);

        Response<Rules> InsertRule(Rules rule);
        Response<Device> InsertDevice(Device device);
        Response<Telemetries> InsertTelemetry(Telemetries telemetry);
        Response<DeviceGroup> InsertGroup(DeviceGroup rule);

        Response<List<Rules>> GetRulesList();
        Response<List<Device>> GetDevicesList();
        Response<List<DeviceGroup>> GetGroupsList();
        Task<Response<List<Device>>> GetDevicesListAsync();
        Response<DeviceTelemetry> GetDeviceTelemetries(Device device);

        Response<bool> RemoveRule(Rules rule);
        Response<bool> RemoveDevice(Device device);
        Response<bool> RemoveGroup(DeviceGroup group);
        Response<bool> RemoveTelemetery(Telemetries telemetry);
    }
}
