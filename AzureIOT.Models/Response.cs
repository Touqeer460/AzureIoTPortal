using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AzureIOT.Models
{
    public class Response
    {
        public bool Success { get; set; }
        public object ResponseObject { get; set; }
        public AzureException[] Exceptions { get; set; }
    }

    public class Response<T>
    {
        public bool Success { get; set; }
        public T ResponseObject { get; set; }
        public AzureException[] Exceptions { get; set; }
    }
}
