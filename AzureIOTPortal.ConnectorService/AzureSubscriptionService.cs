using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureIOT.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.Shared;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using Device = AzureIOT.Models.Device;
using RegistryManager = Microsoft.Azure.Devices.RegistryManager;

namespace AzureIOT.ConnectorService
{
    public class AzureSubscriptionService : ISubscriptionService
    {
        RegistryManager registryManager;
        private CloudBlockBlob blob;
        private string connectionStringForPortal;
        private string connectionStringForStorage;
        private const string ruleKey = "Rules.json", telemetryKey = "Telemetries.json", groupKey = "Groups.json";
        private Response<List<Device>> deviceResponse { get; set; }

        private Response<List<Telemetries>> telemetriesResponse { get; set; }

        public AzureSubscriptionService()
        {
            this.connectionStringForPortal = "HostName=iotHubgldwcmyk4hlua.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=SbkvNpL+vnsWjDfeTk28uIHsN6hwCF2AOFdjZ6rZOAU=";
            this.connectionStringForStorage = "DefaultEndpointsProtocol=https;AccountName=storagegldwcmyk4hlua;AccountKey=verwBcgOlLj9O3QMKtpUbxKbnJpOSOEK/PYdao40Pxt3Lta0xyr1FC9/8ZfXu3IISlSMU+ggiyGjU389o1JqvA==;EndpointSuffix=core.windows.net";
            registryManager = RegistryManager.CreateFromConnectionString(connectionStringForPortal);


        }
        public void GetDeviceListFromAzure()
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionStringForPortal);
            List<Device> localDevices = new List<Device>();
            IEnumerable<Microsoft.Azure.Devices.Device> azureDevice = new List<Microsoft.Azure.Devices.Device>();// = registryManager.GetDevicesAsync(100);
            Task task = Task.Run(() => { azureDevice = registryManager.GetDevicesAsync(100).Result; });
            task.Wait();
            Device currentDevice;
            try
            {
                foreach (var x in azureDevice)
                {
                    currentDevice = new Device()
                    {
                        Id = x.Id,
                        Status = (AzureIOT.Models.Status)x.Status,
                        LastActive = x.ConnectionStateUpdatedTime,
                        CloudToDeviceMessages = x.CloudToDeviceMessageCount,
                        //AuthType = (AzureIOT.Models.AuthTypes)(x. ?? x.AuthenticationType),
                        PrimaryThumbprint = x.Authentication != null ? x.Authentication.X509Thumbprint.PrimaryThumbprint : null,
                        SecondaryThumbprint = x.Authentication != null ? x.Authentication.X509Thumbprint.SecondaryThumbprint : null,
                    };
                    localDevices.Add(currentDevice);
                    currentDevice = null;
                }
                this.deviceResponse = new Response<List<Device>>()
                {
                    Success = true,
                    ResponseObject = localDevices
                };
            }
            catch (Exception ex)
            {
                List<AzureException> azureException = new List<AzureException>() { new AzureException() { Exception = ex, Message = ex.Message } };
                this.deviceResponse = new Response<List<Device>>()
                {
                    Success = false,
                    Exceptions = azureException.ToArray()
                };
            }
        }


        public Response<Device> AddDeviceAsync(Device device)
        {

            Microsoft.Azure.Devices.Device requestDevice;
            try
            {
                //Console.WriteLine("New device:");

                requestDevice = new Microsoft.Azure.Devices.Device(device.Name);
                Task task = Task.Run(() =>
                {
                    requestDevice = registryManager.AddDeviceAsync(requestDevice).Result;
                });
                task.Wait();
                device.Name = requestDevice.Id;
                return new Response<Device>()
                {
                    Success = true,
                    ResponseObject = device
                };
            }
            catch (DeviceAlreadyExistsException ex)
            {
                List<AzureException> exception = new List<AzureException>() { new AzureException() { Exception = ex, Message = ex.Message } };
                return new Response<Device>()
                {
                    Success = true,
                    ResponseObject = device,
                    Exceptions = exception.ToArray()
                };
            }
        }

        public async Task<Response<List<Device>>> GetDevicesListAsync()
        {
            this.GetDeviceListFromAzure();
            return this.deviceResponse;
        }

        public Response<List<Device>> GetDevicesList()
        {
            this.GetDeviceListFromAzure();
            return this.deviceResponse;
        }

        public Response<DeviceTelemetry> GetDeviceTelemetries(Device device)
        {
            throw new NotImplementedException();
        }

        public Response<List<Telemetries>> GetTelemetries()
        {


            return this.GetTelemetriesData();
            ////Get Telemetries from somewhere and populate in Response object.
            //return new Response<List<Telemetries>>()
            //{

            //    Success = true,
            //    ResponseObject = new List<Telemetries>()
            //};
        }

        public Response<Device> InsertDevice(Device device)
        {
            device.Id = device.Name.Replace(" ", "");
            return this.AddDeviceAsync(device);
        }

        public async Task<Response<Telemetries>> InsertTelemetryAsync(Telemetries telemetry)
        {
            SetupBlob(telemetryKey);
            if (blob.Exists())
            {
                var telemetryData = blob.DownloadText();
                List<Telemetries> myList = JsonConvert.DeserializeObject<List<Telemetries>>(telemetryData);
                myList.Add(new Telemetries(telemetry.telemeteryId, telemetry.telemeteryName, telemetry.telemeteryUnit));
                string data = JsonConvert.SerializeObject(myList);
                await blob.UploadTextAsync(data);
            }
            return new Response<Telemetries>()
            {
                Success = true,
                ResponseObject = telemetry
            };
        }
        Response<Device> ISubscriptionService.GetDeviceDetailAsync(string id)
        {
            throw new NotImplementedException();
        }

        private bool SetupBlob(string key)
        {
            try
            {
                string storage = "storagekaispe";
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.connectionStringForStorage);
                CloudBlobClient client = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference(storage);
                container.CreateIfNotExists();
                blob = container.GetBlockBlobReference(key);
                if (!blob.Exists())
                {
                    Task t = Task.Run(() => { blob.UploadTextAsync("[]"); });
                    t.Wait();
                    blob = container.GetBlockBlobReference(key);
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

        Response<Telemetries> ISubscriptionService.InsertTelemetry(Telemetries telemetry)
        {
            Task insert = Task.Run(() => { this.InsertTelemetryAsync(telemetry); });
            insert.Wait();
            return new Response<Telemetries>() { Success = true, ResponseObject = telemetry };
        }

        public Response<List<Telemetries>> GetTelemetriesData()
        {
            SetupBlob(telemetryKey);
            List<Telemetries> localTelemetries = new List<Telemetries>();
            var telemeteryData = blob.DownloadText();
            List<Telemetries> myList = JsonConvert.DeserializeObject<List<Telemetries>>(telemeteryData);

            try
            {
                foreach (var x in myList)
                {
                    localTelemetries.Add(x);
                }

                return this.telemetriesResponse = new Response<List<Telemetries>>()
                {
                    Success = true,
                    ResponseObject = localTelemetries
                };
            }
            catch (Exception ex)
            {
                List<AzureException> azureException = new List<AzureException>() { new AzureException() { Exception = ex, Message = ex.Message } };
                this.telemetriesResponse = new Response<List<Telemetries>>()
                {
                    Success = false,
                    Exceptions = azureException.ToArray()
                };
            }
            return telemetriesResponse;
        }

        public Response<List<Rules>> GetRulesList()
        {
            SetupBlob(ruleKey);
            List<Rules> localRules = new List<Rules>();
            var rule = blob.DownloadText();
            List<Rules> myList = JsonConvert.DeserializeObject<List<Rules>>(rule);

            try
            {
                foreach (var x in myList)
                {
                    localRules.Add(x);
                }
                return new Response<List<Rules>>()
                {
                    Success = true,
                    ResponseObject = localRules
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;// = new Response<List<Telemetries>>()
        }

        public Response<Rules> InsertRule(Rules rule)
        {
            SetupBlob(ruleKey);
            if (blob.Exists())
            {
                var rulesData = blob.DownloadText();
                List<Rules> myList = JsonConvert.DeserializeObject<List<Rules>>(rulesData);
                myList.Add(rule);
                string data = JsonConvert.SerializeObject(myList);
                Task t = Task.Run(() => { blob.UploadTextAsync(data); });
                t.Wait();
            }
            return new Response<Rules>()
            {
                Success = true,
                ResponseObject = rule
            };
        }

        public Response<DeviceGroup> InsertGroup(DeviceGroup group)
        {
            SetupBlob(ruleKey);
            if (blob.Exists())
            {
                var groupsData = blob.DownloadText();
                List<DeviceGroup> myList = JsonConvert.DeserializeObject<List<DeviceGroup>>(groupsData);
                myList.Add(group);
                string data = JsonConvert.SerializeObject(myList);
                Task t = Task.Run(() => { blob.UploadTextAsync(data); });
                t.Wait();
            }
            return new Response<DeviceGroup>()
            {
                Success = true,
                ResponseObject = group
            };
        }

        public Response<List<DeviceGroup>> GetGroupsList()
        {
            SetupBlob(ruleKey);
            List<DeviceGroup> localGroups = new List<DeviceGroup>();
            var groups = blob.DownloadText();
            List<DeviceGroup> myList = JsonConvert.DeserializeObject<List<DeviceGroup>>(groups);

            try
            {
                foreach (var x in myList)
                {
                    localGroups.Add(x);
                }
                return new Response<List<DeviceGroup>>()
                {
                    Success = true,
                    ResponseObject = localGroups
                };
            }
            catch (Exception ex)
            {
                throw ex;
            }
            return null;// = new Response<List<Telemetries>>()
        }
    }
}