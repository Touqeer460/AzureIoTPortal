using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Models
{
    public class Telemetries
    {
        public Telemetries() { }

        public Telemetries(string Id, string Name, string Unit)
        {
            this.telemeteryId = Id;
            this.telemeteryName = Name;
            this.telemeteryUnit = Unit;
        }

        [Key]
        public string telemeteryId { get; set; }
        public string telemeteryName { get; set; }
        public string telemeteryUnit { get; set; }
    }
}
