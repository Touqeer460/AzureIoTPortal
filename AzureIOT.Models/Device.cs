using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Models
{
    public class Device
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Group { get; set; }
        public Status Status { get; set; }
        public DateTime LastActive { get; set; }
    }

    public enum Status
    {
        Active = 1,
        Suspended = 0,
        NonFunctional = 2,
        Fault = 3
    }

}
