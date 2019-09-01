using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Models
{
    public class DashboardFacts
    {
        public int NumberOfRules { get; set; }
        public int OfflineDevices { get; set; }
        public int ConnectedDevices { get; set; }
        public int TotalDevices { get; set; }
        public int TotalDeviceToCloudMessages { get; set; }
        public int TotalCloudToDeviceMessages { get; set; }
    }
}
