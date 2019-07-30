using AzureIOT.DAL.DataProvider;
using AzureIOT.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace AzureIoT.FrontEnd.Controllers
{
    public class TablesController : Controller
    {
        private IDataService dataService;

        public TablesController()
        {
            dataService = new DataProviderFile();
        }

        //
        // GET: /Table/
        public ActionResult Index()
        {
            List<DeviceGroup> groups = dataService.GetAllGroups();
            ViewBag.Groups = groups;
            return View(dataService.GetAllDevices());
        }

        public ActionResult Telemetries()
        {
            return View(dataService.GetTelemetries());
        }

        public ActionResult DeviceGroups()
        {
            return View(dataService.GetAllGroups());
        }

        public bool AddDeviceGroup(DeviceGroup group)
        {
            return dataService.InsertGroup(group);
        }

        public bool AddDevice(Device device)
        {
            return dataService.InsertDevice(device);
        }

        public bool AddTelemetry(Telemetries telemetry)
        {
            return dataService.InsertTelemetry(telemetry);
        }
    }
}