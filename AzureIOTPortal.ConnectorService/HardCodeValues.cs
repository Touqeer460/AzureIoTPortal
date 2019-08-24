using AzureIOT.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AzureIOT.ConnectorService
{
    public class HardCodeValues //: ISubscriptionService
    {
        public Response<Device> GetDeviceDetail(string id)
        {
            return new Response<Device>()
            {
                Success = true,
                ResponseObject = new Device() { Id = id, Name = "New Device", Group = "GRP-1", LastActive = DateTime.Now, Status = Status.Enabled }
            };
        }

        public Response<Device> GetDeviceDetailAsync(string id)
        {
            throw new NotImplementedException();
        }

        public Response<List<Device>> GetDevicesList()
        {
            return new Response<List<Device>>()
            {
                Success = true,
                ResponseObject = new List<Device>()
                {
                    new Device() { Id = "Rotary-01", Name = "Alto 45433", Group = "Rotary", LastActive = new DateTime(2019, 08, 03, 12, 0, 0), Status = Status.Enabled },
                    new Device() { Id = "Rotary-05", Name = "Alto 45321", Group = "Rotary", LastActive = new DateTime(2019, 08, 01, 10, 0, 0), Status = Status.Disabled },
                    new Device() { Id = "Centrifugal-02", Name = "Chang 102001", Group = "Centrifugal", LastActive = new DateTime(2019, 08, 07, 14, 0, 0), Status = Status.Enabled },
                    new Device() { Id = "Centrifugal-05", Name = "Hugo 901", Group = "Centrifugal", LastActive = new DateTime(2019, 08, 07, 11, 0, 0), Status = Status.Enabled },
                    new Device() { Id = "Centrifugal-09", Name = "Hugo 902", Group = "Centrifugal", LastActive = new DateTime(2019, 08, 07, 11, 0, 0), Status = Status.Enabled },
                }
            };
        }

        public Task<Response<List<Device>>> GetDevicesListAsync()
        {
            throw new NotImplementedException();
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
                        new TelemetryValue(){ Telemetry = new Telemetries(){ telemeteryName = "elevatorSensors:v1"}, Value = 5.0}
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
                        new Telemetries(){ telemeteryId = "IP", telemeteryName = "Inlet Pressure", telemeteryUnit = "psi" },
                        new Telemetries(){ telemeteryId = "Temp", telemeteryName = "Temperature", telemeteryUnit = "F" },
                        new Telemetries(){ telemeteryId = "cfm", telemeteryName = "CFM", telemeteryUnit = "m3/min" },
                        new Telemetries(){ telemeteryId = "mp", telemeteryName = "Motor Power", telemeteryUnit = "HP" }
                    }
            };
        }

        public Response<Device> InsertDevice(Device device)
        {
            return new Response<Device>()
            {
                Success = true,
                ResponseObject = new Device() { Id = device.Id, LastActive = DateTime.MinValue, Status = Status.Disabled }
            };
        }

        public Response<Telemetries> InsertTelemetry(Telemetries telemetry)
        {
            return new Response<Telemetries>()
            {
                Success = true,
                ResponseObject = new Telemetries() { telemeteryId = telemetry.telemeteryId }
            };
        }
    }
}
