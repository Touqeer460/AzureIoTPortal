using AzureIOT.Models;
using AzureIOT.ConnectorService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace AzureIOT.DAL.DataProvider
{
    public class DataProviderFile : IDataService
    {
        string folderPathToStoreInfo;
        const string format = ".json", devicePath = "devices", groupPath = "groups", telemetriesPath = "telemetries", rulesPath = "rules";
        ISubscriptionService subscription;

        public DataProviderFile(string constr = "")//Change this if you want.
        {
            if (constr == "")
            {
                constr = ConfigurationManager.AppSettings["DataPath"];
            }
            this.Connect(constr);
            //Farrukh you have to change this HardCardValue to your class. new AzureSubscriptionClass
            subscription = new AzureSubscriptionService();
        }

        public bool Connect(string constr)
        {
            this.folderPathToStoreInfo = constr;
            return true;
        }

        public async Task<Response<List<Device>>> GetAzureDevice()
        {
            return await subscription.GetDevicesListAsync();
        }

        public async Task<List<Device>> GetAllDevices()
        {
            string relativePath = $"{folderPathToStoreInfo}\\{devicePath}{format}";
            List<Device> azureDevices = subscription.GetDevicesList().ResponseObject;
            List<Device> localDevices = JsonConvert.DeserializeObject<List<Device>>(File.ReadAllText(relativePath)) ?? new List<Device>();
            azureDevices = azureDevices ?? new List<Device>();
            azureDevices.ForEach(x =>
            {
                if (localDevices.All(r => r.Id != x.Id))
                {
                    localDevices.Add(x);
                    this.InsertDeviceFileOnly(x);
                }
            });

            return localDevices;
        }

        public List<DeviceGroup> GetAllGroups()
        {
            string relativePath = $"{folderPathToStoreInfo}\\{groupPath}{format}";
            List<DeviceGroup> savedGroups = subscription.GetGroupsList().ResponseObject;
            List<DeviceGroup> localGroups = this.GetAllDeviceGroupsForced();
            savedGroups = savedGroups ?? new List<DeviceGroup>();
            savedGroups.ForEach(x =>
            {
                if (localGroups.All(r => r.Id != x.Id))
                {
                    localGroups.Add(x);
                    this.InsertDeviceGroupFileOnly(x);
                }
            });
            return localGroups;
        }

        public Device GetDeviceInfo(string id)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{devicePath}{format}";
            List<Device> data = JsonConvert.DeserializeObject<List<Device>>(File.ReadAllText(relativePath));
            if (data.Where(x => x.Id == id) != null)
                return data.Where(x => x.Id == id).First();
            return null;
        }

        public DeviceGroup GetGroupInfo(string id)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{groupPath}{format}";
            List<DeviceGroup> data = JsonConvert.DeserializeObject<List<DeviceGroup>>(File.ReadAllText(relativePath));
            if (data.Where(x => x.Id == id) != null)
                return data.Where(x => x.Id == id).First();
            return null;
        }

        public List<Telemetries> GetTelemetries()
        {
            string relativePath = $"{folderPathToStoreInfo}\\{telemetriesPath}{format}";
            List<Telemetries> azureTelemetries = subscription.GetTelemetries().ResponseObject;
            List<Telemetries> localTelemetries = new List<Telemetries>();
            azureTelemetries = azureTelemetries ?? new List<Telemetries>();
            azureTelemetries.ForEach(x =>
            {
                if (x.telemeteryId != "" && localTelemetries.All(r => r.telemeteryId != x.telemeteryId))
                {
                    localTelemetries.Add(x);
                    this.InsertTelemetry(x);
                }
            });
            return localTelemetries;
        }

        public Telemetries GetTelemetryInfo(string id)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{telemetriesPath}{format}";
            List<Telemetries> data = JsonConvert.DeserializeObject<List<Telemetries>>(File.ReadAllText(relativePath));
            if (data.Where(x => x.telemeteryId == id) != null)
                return data.Where(x => x.telemeteryId == id).First();
            return null;
        }

        public bool InsertGroup(DeviceGroup group)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{groupPath}{format}";
            List<DeviceGroup> groups = this.GetAllDeviceGroupsForced();
            subscription.InsertGroup(group);
            groups.Add(group);
            File.WriteAllText(relativePath, JsonConvert.SerializeObject(groups));
            return true;
        }

        public bool InsertDevice(Device device)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{devicePath}{format}";
            List<Device> devices = this.GetAllDevicesForced();
            Response<Device> insert = subscription.InsertDevice(device);
            if (insert.Success)
            {
                device.Id = insert.ResponseObject.Id;
                device.LastActive = insert.ResponseObject.LastActive;
                device.Status = insert.ResponseObject.Status;

                devices.Add(device);
                File.WriteAllText(relativePath, JsonConvert.SerializeObject(devices));
                return true;
            }
            return false;
        }

        public bool InsertTelemetry(Telemetries telemetry)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{telemetriesPath}{format}";
            List<Telemetries> telemetries = this.GetAllTelemetriesForced();
            Response<Telemetries> insert = subscription.InsertTelemetry(telemetry);
            if (insert.Success)
            {
                telemetry.telemeteryId = insert.ResponseObject.telemeteryId;

                telemetries.Add(telemetry);
                File.WriteAllText(relativePath, JsonConvert.SerializeObject(telemetries));
                return true;
            }
            return false;
        }

        public List<Device> GetAllDevicesForced()
        {
            string relativePath = $"{folderPathToStoreInfo}\\{devicePath}{format}";
            List<Device> localDevices = JsonConvert.DeserializeObject<List<Device>>(File.ReadAllText(relativePath)) ?? new List<Device>();
            return localDevices;
        }

        public bool InsertDeviceFileOnly(Device device)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{devicePath}{format}";
            List<Device> devices = this.GetAllDevicesForced();

            devices.Add(device);
            File.WriteAllText(relativePath, JsonConvert.SerializeObject(devices));
            return true;
        }

        public List<Telemetries> GetAllTelemetriesForced()
        {
            string relativePath = $"{folderPathToStoreInfo}\\{telemetriesPath}{format}";
            List<Telemetries> localTelemetries = JsonConvert.DeserializeObject<List<Telemetries>>(File.ReadAllText(relativePath)) ?? new List<Telemetries>();
            return localTelemetries;
        }

        public bool InsertTelemetryFileOnly(Telemetries telemetry)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{telemetriesPath}{format}";
            List<Telemetries> telemetries = this.GetAllTelemetriesForced();

            telemetries.Add(telemetry);
            File.WriteAllText(relativePath, JsonConvert.SerializeObject(telemetries));
            return true;
        }

        public List<DeviceGroup> GetAllDeviceGroupsForced()
        {
            string relativePath = $"{folderPathToStoreInfo}\\{groupPath}{format}";
            List<DeviceGroup> localGroups = JsonConvert.DeserializeObject<List<DeviceGroup>>(File.ReadAllText(relativePath)) ?? new List<DeviceGroup>();
            return localGroups;
        }

        public bool InsertDeviceGroupFileOnly(DeviceGroup group)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{groupPath}{format}";
            List<DeviceGroup> groups = this.GetAllDeviceGroupsForced();

            groups.Add(group);
            File.WriteAllText(relativePath, JsonConvert.SerializeObject(groups));
            return true;
        }

        public List<Rules> GetAllRules()
        {
            string relativePath = $"{folderPathToStoreInfo}\\{rulesPath}{format}";
            List<Rules> azureRules = subscription.GetRulesList().ResponseObject;
            List<Rules> localRules = JsonConvert.DeserializeObject<List<Rules>>(File.ReadAllText(relativePath)) ?? new List<Rules>();
            azureRules.ToList().ForEach(x => 
            {
                if (x.Id != "" && localRules.All(r=>r.Id != x.Id))
                {
                    this.InsertRuleForced(x);
                }
            });
            return localRules;
        }

        public Rules GetRuleInfo(string id)
        {
            List<Rules> localRules = this.GetAllRules();
            if (localRules.Count() > 0)
            {
                IEnumerable<Rules> rule = localRules.Where(x => x.Id == id);
                return rule != null ? rule.First() : new Rules();
            }
            return new Rules();
        }

        public bool InsertRule(Rules rule)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{rulesPath}{format}";
            subscription.InsertRule(rule);
            List<Rules> rules = this.GetAllRules();

            rules.Add(rule);
            File.WriteAllText(relativePath, JsonConvert.SerializeObject(rules));
            return true;
        }

        public bool InsertRuleForced(Rules rule)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{rulesPath}{format}";
            List<Rules> rules = JsonConvert.DeserializeObject<List<Rules>>(File.ReadAllText(relativePath)) ?? new List<Rules>();

            rules.Add(rule);
            File.WriteAllText(relativePath, JsonConvert.SerializeObject(rules));
            return true;
        }

        public bool RemoveRule(Rules rule)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{rulesPath}{format}";
            List<Rules> localRules = this.GetAllRules();
            if (subscription.RemoveRule(rule).Success)
            {
                rule = localRules.Find(x => x.Id == rule.Id);
                if (rule != null)
                {
                    localRules.Remove(rule);
                    File.WriteAllText(relativePath, JsonConvert.SerializeObject(localRules));
                }
            }
            return false;
        }

        public bool RemoveGroup(DeviceGroup group)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{groupPath}{format}";
            List<DeviceGroup> localGroups = this.GetAllDeviceGroupsForced();
            if (subscription.RemoveGroup(group).Success)
            {
                group = localGroups.Find(x => x.Id == group.Id);
                if (group != null)
                {
                    localGroups.Remove(group);
                    File.WriteAllText(relativePath, JsonConvert.SerializeObject(localGroups));
                }
            }
            return false;
        }

        public bool RemoveDevice(Device device)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{devicePath}{format}";
            List<Device> localDevices = this.GetAllDevicesForced();
            if (subscription.RemoveDevice(device).Success)
            {
                device = localDevices.Find(x => x.Id == device.Id);
                if (device != null)
                {
                    localDevices.Remove(device);
                    File.WriteAllText(relativePath, JsonConvert.SerializeObject(localDevices));
                }
            }
            return false;
        }

        public bool RemoveTelemetry(Telemetries telemetry)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{telemetriesPath}{format}";
            List<Telemetries> localTelemetries = this.GetAllTelemetriesForced();
            if (subscription.RemoveTelemetery(telemetry).Success)
            {
                telemetry = localTelemetries.Find(x => x.telemeteryId == telemetry.telemeteryId);
                if (telemetry != null)
                {
                    localTelemetries.Remove(telemetry);
                    File.WriteAllText(relativePath, JsonConvert.SerializeObject(localTelemetries));
                }
            }
            return false;
        }
    }
}
