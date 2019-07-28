using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Models
{
    public class DeviceGroup
    {
        [Key]
        public string Id { get; set; }
        public string Name { get; set; }

        public List<Telemetries> Telemetries { get; set; }
    }
}
