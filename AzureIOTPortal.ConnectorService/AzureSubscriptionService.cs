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
            return this.AddDeviceAsync(device);
        }

        public async Task<Response<Telemetries>> InsertTelemetryAsync(Telemetries telemetry)
        {

            //throw new NotImplementedException();
            SetupBlob();
            //telemetry.telemetryId = telemetry.telemetryId;
            //telemetry.telemetryName = telemetry.telemetryName;
            //telemetry.telemetryUnit = telemetry.telemetryUnit;

            //myList.Add(new Telemetries(telemetry.telemetryId, telemetry.telemetryName, telemetry.telemetryUnit));


            //Your code here - 8/6/2019
            if (blob.Exists())
            {
                var telemetryData = blob.DownloadText();
                //string jsonSTRING = File.ReadAllText(customerData);
                List<Telemetries> myList = JsonConvert.DeserializeObject<List<Telemetries>>(telemetryData);
                //telemetry.telemetryId = this.InsertTelemetry(telemetry);
                //telemetriesResponse.Id = telemetriesResponse.Id;
                //telemetriesResponse.Name = telemetriesResponse.Name;
                //telemetriesResponse.Unit = telemetriesResponse.Unit;
                telemetry.Id = telemetry.Id;
                telemetry.Name = telemetry.Name;
                telemetry.Unit = telemetry.Unit;
                //myList.Add(new Telemetries(telemetriesResponse.Id, telemetriesResponse.Name, telemetriesResponse.Unit));
                myList.Add(new Telemetries(telemetry.Id,telemetry.Name,telemetry.Unit));
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

        private bool SetupBlob()
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.connectionStringForStorage);
                CloudBlobClient client = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference("storagekaispe");
                container.CreateIfNotExists();
                string key = @"myCode.json";
                blob = container.GetBlockBlobReference(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        Response<Telemetries> ISubscriptionService.InsertTelemetry(Telemetries telemetry)
        {

            throw new NotImplementedException();
            //return this.TelemetryInsertAsync();
        }

       
        private bool SetupBlob2()
        {
            try
            {
                CloudStorageAccount storageAccount = CloudStorageAccount.Parse(this.connectionStringForStorage);
                CloudBlobClient client = storageAccount.CreateCloudBlobClient();
                CloudBlobContainer container = client.GetContainerReference("storagekaispe");
                container.CreateIfNotExists();
                string key = @"Rules3.json";
                blob = container.GetBlockBlobReference(key);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public void GetTelemetries2()
        {
            //SetupBlob2();
            //if (blob.Exists())
            //{
            //    var customerData2 = blob.DownloadText();
            //    List<Rules> myRules = JsonConvert.DeserializeObject<List<Rules>>(customerData2);

            //    foreach (var telemeteryData in myRules)
            //    {
            //        telemetry.
                    
            //    }
            //}

        }

        //public Response<List<Telemetries>> GetTelemetriesValue()
        //{
        //    var telemeteryData = blob.DownloadText();
        //    List<Telemetries> myList = JsonConvert.DeserializeObject<List<Telemetries>>(telemeteryData);
        //    Telemetries telemetries;
        //    foreach (var telemeteryValue in myList)

        //    {
        //        string telemeteryId = telemeteryValue.Id;
        //        string telemeteryName = telemeteryValue.Name;
        //        string telemeteryUnit = telemeteryValue.Unit;
        //    }

        //    //myList.Add();
        //    this.telemetriesResponse= new Response<List<Telemetries>>()
        //    {
        //        Success = true,
        //       ResponseObject=myList
        //    };
        //}

        public Response<List<Telemetries>> GetTelemetriesData()
        {
            SetupBlob();
            List<Telemetries> localTelemetries = new List<Telemetries>();
            var telemeteryData = blob.DownloadText();
            List<AzureTelemetryWrapper> myList = JsonConvert.DeserializeObject<List<AzureTelemetryWrapper>>(telemeteryData);
            
            try
            {
                
                foreach (var x in myList)
                {
                    localTelemetries.Add(new Telemetries()
                    {
                        Id = x.telemeteryId,
                        Name = x.telemeteryName,
                        Unit = x.telemeteryUnit
                    });
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
            return telemetriesResponse;// = new Response<List<Telemetries>>()
            //{
            //    Success = true,
            //    ResponseObject = localTelemetries
            //};
        }

        public async Task TelemetryInsertAsync(Telemetries telemetry)
        {
            //SetupBlob();
            //var telemeteryData = blob.DownloadText();
            //////string jsonSTRING = File.ReadAllText(customerData);

            //List<Telemetries> myList = JsonConvert.DeserializeObject<List<Telemetries>>(telemeteryData);
            //if (myList == null)
            //    myList = new List<Telemetries>();
            //////string telemetryId = telemetry.Id;
            //////string telemetryName = telemetry.Name;
            //////string telemetryUnit = telemetry.Unit;
            //myList.Add(new Telemetries(telemetry.Id, telemetry.Name, telemetry.Unit));

            //string data = JsonConvert.SerializeObject(myList);
            //await blob.UploadTextAsync(data);
        }

    }

    public class AzureTelemetryWrapper
    {
        public string telemeteryId { get; set; }
        public string telemeteryName { get; set; }
        public string telemeteryUnit { get; set; }
    }
}