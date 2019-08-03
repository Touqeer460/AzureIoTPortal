using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AzureIOT.Models;
using Microsoft.Azure.Devices;
using Microsoft.Azure.Devices.Common.Exceptions;
using Microsoft.Azure.Devices.Shared;
using Device = AzureIOT.Models.Device;
using RegistryManager = Microsoft.Azure.Devices.RegistryManager;

namespace AzureIOT.ConnectorService
{
    public class AzureSubscriptionService : ISubscriptionService
    {

        RegistryManager registryManager;
        public string connectionString;

        private Response<List<Device>> deviceResponse { get; set; }

        public AzureSubscriptionService(string constr = "HostName=iotHub4jbagz366jxyq.azure-devices.net;SharedAccessKeyName=iothubowner;SharedAccessKey=ijus5i/Z0Pr0EFHFQuNcK4kpqI+34rPmgz+VbTHFZUw=")
        {
            this.connectionString = constr;
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
        }
        public void GetDeviceListFromAzure()
        {
            registryManager = RegistryManager.CreateFromConnectionString(connectionString);
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
            throw new NotImplementedException();
        }

        public Response<Device> InsertDevice(Device device)
        {
            return this.AddDeviceAsync(device);
        }

        public Response<Telemetries> InsertTelemetry(Telemetries telemetry)
        {
            throw new NotImplementedException();
        }

        Response<Device> ISubscriptionService.GetDeviceDetailAsync(string id)
        {
            throw new NotImplementedException();
        }

        //new Code
    }
}
