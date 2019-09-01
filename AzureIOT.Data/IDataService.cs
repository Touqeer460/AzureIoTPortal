using AzureIOT.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.DAL.DataProvider
{
    public interface IDataService
    {
        bool Connect(string constr);

        Rules GetRuleInfo(string id);
        Device GetDeviceInfo(string id);
        Telemetries GetTelemetryInfo(string id);
        DeviceGroup GetGroupInfo(string id);

        bool InsertRule(Rules rule);
        bool InsertGroup(DeviceGroup group);
        bool InsertDevice(Device device);
        bool InsertTelemetry(Telemetries telemetry);

        List<Rules> GetAllRules();
        List<DeviceGroup> GetAllGroups();
        Task<List<Device>> GetAllDevices();
        List<Telemetries> GetTelemetries();

        bool RemoveRule(Rules rule);
        bool RemoveGroup(DeviceGroup group);
        bool RemoveDevice(Device device);
        bool RemoveTelemetry(Telemetries telemetry);
    }
}
