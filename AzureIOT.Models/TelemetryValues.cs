using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Models
{
    public class TelemetryValue
    {
        public Telemetries Telemetry { get; set; }
        public double   Value { get; set; }
    }
}
