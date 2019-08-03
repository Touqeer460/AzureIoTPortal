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
        public int CloudToDeviceMessages { get; set; }
        public AuthTypes AuthType { get; set; }
        public string PrimaryThumbprint { get; set; }
        public string SecondaryThumbprint { get; set; }
    }

    public enum Status
    {
        Enabled = 0,
        Disabled = 1
    }

    public enum AuthTypes
    {
        Sas = 0,
        SelfSigned = 1,
        CertificateAuthority = 2,
        None = 3
    }

}
