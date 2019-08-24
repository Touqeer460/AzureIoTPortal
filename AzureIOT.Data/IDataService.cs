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
        Task<List<Device>> GetAllDevices();
        Device GetDeviceInfo(string id);
        List<Telemetries> GetTelemetries();
        Telemetries GetTelemetryInfo(string id);
        List<DeviceGroup> GetAllGroups();
        List<Rules> GetAllRules();
        Rules GetRuleInfo(string id);
        DeviceGroup GetGroupInfo(string id);
        bool InsertGroup(DeviceGroup group);
        bool InsertDevice(Device device);
        bool InsertTelemetry(Telemetries telemetry);
        bool InsertRule(Rules rule);
    }
}
