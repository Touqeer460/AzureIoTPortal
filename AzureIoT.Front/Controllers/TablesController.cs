using AzureIOT.DAL.DataProvider;
using AzureIOT.Models;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
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
        public async Task<ActionResult> Index()
        {
            List<DeviceGroup> groups = dataService.GetAllGroups();
            ViewBag.Groups = groups;
            return View(await dataService.GetAllDevices());
        }

        public ActionResult Telemetries()
        {
            return View(dataService.GetTelemetries());
        }

        public ActionResult DeviceGroups()
        {
            List<Telemetries> telemetries = dataService.GetTelemetries();
            ViewBag.Telemetries = telemetries;
            return View(dataService.GetAllGroups());
        }

        public bool AddDeviceGroup(DeviceGroup group)
        {
            if (group != null && group.Telemetries != null && group.Telemetries.Length == 1)
            {
                group.Telemetries = group.Telemetries[0].Split(',');
            }
            return dataService.InsertGroup(group);
        }

        public ActionResult RuleDetail(string Id)
        {
            List<Rules> rules = dataService.GetAllRules();
            return View(rules.Find(x => x.Id == Id));
        }

        public bool AddDevice(Device device)
        {
            return dataService.InsertDevice(device);
        }

        public bool AddTelemetry(Telemetries telemetry)
        {
            return dataService.InsertTelemetry(telemetry);
        }

        public bool AddRule(Rules rule)
        {
            return dataService.InsertRule(rule);
        }

        public ActionResult Rules()
        {
            ViewBag.Groups = dataService.GetAllGroups();
            ViewBag.Telemetries = dataService.GetTelemetries();
            ViewBag.Operators = AzureIOT.Models.Rules.AllowedOperators();
            return View(dataService.GetAllRules());
        }

        public ActionResult DeviceGroupDetail(string Id)
        {
            List<DeviceGroup> group = dataService.GetAllGroups();
            return View(group.Find(x => x.Id == Id));
        }
    }
}