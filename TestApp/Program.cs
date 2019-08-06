using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Blob;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;

namespace TestApp
{
    class Program
    {
        static void Main(string[] args)
        {

            //Getting Data from the App Settings
            string connString = "DefaultEndpointsProtocol=https;AccountName=storagegldwcmyk4hlua;AccountKey=verwBcgOlLj9O3QMKtpUbxKbnJpOSOEK/PYdao40Pxt3Lta0xyr1FC9/8ZfXu3IISlSMU+ggiyGjU389o1JqvA==;EndpointSuffix=core.windows.net";
            //string localFolder = ConfigurationManager.AppSettings["sourceFolder"];

            //Creating the Blob Storage Account
            CloudStorageAccount storageAccount = CloudStorageAccount.Parse(connString);

            //creating Blob Client
            CloudBlobClient client = storageAccount.CreateCloudBlobClient();

            //creating Container

            CloudBlobContainer container = client.GetContainerReference("storagekaispe");
            container.CreateIfNotExists();


            string key = @"myCode.json";


            CloudBlockBlob blob = container.GetBlockBlobReference(key);
            if (blob.Exists())
            {
                var telemeteryData = blob.DownloadText();
                //string jsonSTRING = File.ReadAllText(customerData);

                blob.UploadText("") ;
                //if (myList == null)
                //    myList = new List<Telemetery>();

                //telemetery.telemeteryId = TextBox1.Text;
                //telemetery.telemeteryName = TextBox2.Text;
                //telemetery.telemeteryUnit = TextBox3.Text;
                //myList.Add(new Telemetery(telemetery.telemeteryId, telemetery.telemeteryName, telemetery.telemeteryUnit));
                //string data = JsonConvert.SerializeObject(myList);
                //File.WriteAllText("TelemeteryData.json", data);
            }
            else
            {
                blob.UploadText("");

            }
        }
    }
}
