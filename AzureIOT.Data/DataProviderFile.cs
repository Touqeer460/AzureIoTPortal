using AzureIOT.Models;
using AzureIOT.ConnectorService;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.DAL.DataProvider
{
    public class DataProviderFile : IDataService
    {
        string folderPathToStoreInfo;
        const string format = ".json", devicePath = "devices", groupPath = "groups", telemetriesPath = "telemetries";
        ISubscriptionService subscription;

        public DataProviderFile(string constr = "C:\\Data\\")//Change this if you want.
        {
            this.Connect(constr);
            //Farrukh you have to change this HardCardValue to your class. new AzureSubscriptionClass
            subscription = new HardCodeValues();
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
            //Currently deploying this crude way to sync data.
            //Will update later.
            //Todo:
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
            List<DeviceGroup> savedGroups = new List<DeviceGroup>();// this.GetAllDeviceGroupsForced();
            List<DeviceGroup> localGroups = this.GetAllDeviceGroupsForced();
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
            azureTelemetries.ForEach(x =>
            {
                if (localTelemetries.All(r => r.Id != x.Id))
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
            if (data.Where(x => x.Id == id) != null)
                return data.Where(x => x.Id == id).First();
            return null;
        }

        public bool InsertGroup(DeviceGroup group)
        {
            string relativePath = $"{folderPathToStoreInfo}\\{groupPath}{format}";
            List<DeviceGroup> groups = this.GetAllDeviceGroupsForced();
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
                telemetry.Id = insert.ResponseObject.Id;

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
    }
}
