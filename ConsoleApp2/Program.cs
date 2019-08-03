using AzureIOT.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApp2
{
    class Program
    {
        static void Main(string[] args)
        {
            Program.PerformAction();
            Console.ReadKey();
        }

        public async static void PerformAction()
        {
            AzureSubscriptionService test = new AzureSubscriptionService();
            //await test.GetDeviceListFromAzure();
        }
    }
}
