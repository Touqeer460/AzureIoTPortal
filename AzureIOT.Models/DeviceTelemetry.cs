using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Models
{
    public class DeviceTelemetry : Device
    {
        public List<TelemetryValue> Telemetries { get; set; }
    }
}
