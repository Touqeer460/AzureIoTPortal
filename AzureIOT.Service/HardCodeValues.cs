using AzureIOT.Models;
using System;
using System.Collections.Generic;

namespace AzureIOT.Service
{
    internal class HardCodeValues : ISubscriptionService
    {
        public Response<Device> GetDeviceDetail(string id)
        {
            return new Response<Device>()
            {
                Success = true,
                ResponseObject = new Device() { Id = id, Name = "New Device", Group = new DeviceGroup() { Id = "GRP-1", Name = "" }, LastActive = DateTime.Now, Status = Status.Active }
            };
        }

        public Response<List<Device>> GetDevicesList()
        {
            return new Response<List<Device>>()
            {
                Success = true,
                ResponseObject = new List<Device>()
                {
                    new Device() { Id = "dev-1", Name = "New Device 1", Group = new DeviceGroup() { Id = "GRP-1", Name = "" }, LastActive = DateTime.Now, Status = Status.Active },
                    new Device() { Id = "dev-2", Name = "New Device 2", Group = new DeviceGroup() { Id = "GRP-1", Name = "" }, LastActive = DateTime.Now, Status = Status.Active },
                }
            };
        }

        public Response<DeviceTelemetry> GetDeviceTelemetries(Device device)
        {
            return new Response<DeviceTelemetry>()
            {
                Success = true,
                ResponseObject = new DeviceTelemetry()
                {
                    Id = device.Id,
                    Group = device.Group,
                    Name = device.Name,
                    Telemetries = new List<TelemetryValue>()
                    {
                        new TelemetryValue(){ Telemetry = new Telemetries(){ Name = "elevatorSensors:v1"}, Value = 5.0}
                    }
                }
            };
        }

        public Response<List<Telemetries>> GetTelemetries()
        {
            return new Response<List<Telemetries>>()
            {
                Success = true,
                ResponseObject = new List<Telemetries>()
                    {
                        new Telemetries(){ Name = "elevatorSensors:v1" }
                    }
            };
        }
    }
}
