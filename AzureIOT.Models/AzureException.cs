using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Models
{
    public class AzureException
    {
        public Exception Exception { get; set; }
        public string   Message { get; set; }
    }
}
